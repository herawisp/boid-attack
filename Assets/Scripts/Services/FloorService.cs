using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorService : MonoBehaviour {

    public static FloorService Instance {get; private set;}

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    
    //================================================================================================//
    //================================================================================================//

    public DungeonData DungeonData;
    public RectTransform Dungeon;
    public RectTransform Transition;
    public List<EnemyTeamData> EnemyTeams;
    public List<ButterflyType> StarterButterflies = new() {
        ButterflyType.CrypticWoodWhite, ButterflyType.ChalkHillBlue, ButterflyType.OrangeTip
    };
    public Chest Chest;

    //================================================================================================//
    //================================================================================================//

    public void GenerateFloor() {
        int floorNumber = DungeonData?.FloorNumber ?? 1;

        DungeonGenerator dungeonGenerator = new();
        DungeonData = dungeonGenerator.Generate();
        DungeonData.FloorNumber = floorNumber;

        MapService.Instance.UpdateMap();
        UpdateDungeonRoom();
    }
    
    public void StartNewRun() {
        ButterflyService.Instance.ClearButterflies(TeamType.Player);
        ButterflyService.Instance.ClearButterflies(TeamType.Enemy);

        ButterflyService.Instance.SetButterflies(StarterButterflies, TeamType.Player);

        DungeonData = new DungeonData();   // fresh state — FloorNumber resets to its default (you'll want 1)
        GenerateFloor();
    }

    // public void MoveUp()
    // {
    //     TransitionUp(() => {
    //         TryMove(DungeonData.CurrentCell - DungeonData.Width);  
    //     });
    // }

    public void MoveUp()  => TryMove(DungeonData.CurrentCell - DungeonData.Width);
    public void MoveDown()  => TryMove(DungeonData.CurrentCell + DungeonData.Width);
    public void MoveLeft()  => TryMove(DungeonData.CurrentCell - 1);
    public void MoveRight() => TryMove(DungeonData.CurrentCell + 1);

    void TryMove(int targetCell) {
        List<bool> neighbours = GetNeighboursRoom(DungeonData.CurrentCell);
        int direction = targetCell - DungeonData.CurrentCell;
        int width = DungeonData.Width;

        bool canMove =
            (direction == -width && neighbours[0]) || // top
            (direction == width  && neighbours[1]) || // bottom
            (direction == -1     && neighbours[2]) || // left
            (direction == 1      && neighbours[3]);   // right

        if (!canMove) return;

        DungeonData.CurrentCell = targetCell;
        MapService.Instance.UpdateMap();
        UpdateDungeonRoom();
    }

    List<bool> GetNeighboursRoom(int i) {
        int width = DungeonData.Width;
        int x = i % width;
        var cells = DungeonData.Cells;

        return new List<bool> {
            i >= width                && cells[i - width] != RoomType.None,  // top
            i < cells.Length - width  && cells[i + width] != RoomType.None,  // bottom
            x > 0                     && cells[i - 1]     != RoomType.None,  // left
            x < width - 1             && cells[i + 1]     != RoomType.None   // right
        };
    }

    void UpdateDungeonRoom() {
        int cell = DungeonData.CurrentCell;
        RoomType roomType = DungeonData.Cells[cell];

        DungeonData.Visited[cell] = true;
        Chest.gameObject.SetActive(false);

        bool locked = roomType == RoomType.Battle && !DungeonData.Cleared[cell];
        List<bool> doorState = locked ? new List<bool> { false, false, false, false } : GetNeighboursRoom(cell);
        DungeonRoom.Instance.UpdateNormalRoom(doorState);

        switch (roomType) {
            case RoomType.Battle:
                if (!DungeonData.Cleared[cell]) StartBattle(cell);
                break;
            case RoomType.Shop:
                ShopService.Instance.OpenShop();
                break;
            case RoomType.Key:
                DungeonData.HasKey = true;
                DungeonData.Cells[cell] = RoomType.Empty;
                KeyPickupUI.Instance.ShowKeyFound();
                break;
            case RoomType.Exit:
                ShowChest();
                break;
        }

        MapService.Instance.UpdateMap();
    }
    
    //================================================================================================//
    //================================================================================================//
    
    void TransitionUp(TweenCallback onComplete = null) {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(Transition.DOAnchorPosY(600, 1f));
        sequence.AppendCallback(() => Dungeon.anchoredPosition = new Vector2(0, 0));
        sequence.Append(Transition.DOAnchorPosY(-1140, 1f));

        if (onComplete != null) sequence.OnComplete(onComplete);
    }


    //================================================================================================//
    //================================================================================================//

    void TransitionToNextFloor(TweenCallback onComplete) {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Transition.DOAnchorPosY(600, 1f));
        sequence.AppendCallback(() => Dungeon.anchoredPosition = new Vector2(0, 0));
        sequence.Append(Transition.DOAnchorPosY(-1140, 1f));
        if (onComplete != null) sequence.OnComplete(onComplete);
    }

    void ShowChest() {
        Chest.gameObject.SetActive(true);
        Chest.SetState(DungeonData.HasKey);
    }

    public void OpenChest() {
        if (!DungeonData.HasKey) return;   // Chest.OnClicked already guards this, but safe to keep

        TransitionToNextFloor(() => {
            DungeonData.FloorNumber++;
            GenerateFloor();
        });
    }

    public void ConsumeShopRoom() {
        DungeonData.Cells[DungeonData.CurrentCell] = RoomType.Empty;
        DungeonRoom.Instance.UpdateNormalRoom(GetNeighboursRoom(DungeonData.CurrentCell));
    }

    //================================================================================================//
    //================================================================================================//

    void StartBattle(int cell) {
        Difficulty difficulty = PickDifficulty(DungeonData.FloorNumber);
        List<EnemyTeamData> pool = EnemyTeams.FindAll(t => t.Difficulty == difficulty);
        EnemyTeamData team = pool[Random.Range(0, pool.Count)];
        ButterflyService.Instance.SetButterflies(team.Members, TeamType.Enemy);
        ButterflyService.Instance.EnableAllButterfly();

        ButterflyService.Instance.ButterflyDied += OnBattleButterflyDied;
    }

    Difficulty PickDifficulty(int floorNumber) {
        if (floorNumber <= 2) return Difficulty.Easy;
        if (floorNumber <= 5) return Difficulty.Medium;
        return Difficulty.Hard;
    }

    void OnBattleButterflyDied(Butterfly b) {
        StartCoroutine(CheckBattleEndNextFrame());
    }

    IEnumerator CheckBattleEndNextFrame() {
        yield return null;   // let any OnSelfDied-triggered spawns (splits, etc.) resolve first

        int enemyLeft = ButterflyService.Instance.GetButterflies(TeamType.Enemy).FindAll(x => x.gameObject.activeInHierarchy).Count;
        int playerLeft = ButterflyService.Instance.GetButterflies(TeamType.Player).FindAll(x => x.gameObject.activeInHierarchy).Count;

        if (enemyLeft <= 0) {
            ButterflyService.Instance.ButterflyDied -= OnBattleButterflyDied;
            DungeonData.Cleared[DungeonData.CurrentCell] = true;
            ButterflyService.Instance.ClearSpawnedButterflies(TeamType.Enemy);
            ButterflyService.Instance.DisableAllButterfly();
            DungeonRoom.Instance.UpdateNormalRoom(GetNeighboursRoom(DungeonData.CurrentCell));
        } else if (playerLeft <= 0) {
            ButterflyService.Instance.ButterflyDied -= OnBattleButterflyDied;
            GameOver();
        }
    }
    
    void GameOver() {
        // simplest version — restart the run entirely
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

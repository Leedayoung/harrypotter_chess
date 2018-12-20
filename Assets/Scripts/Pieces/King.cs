using UnityEngine;
using UnityEngine.UI;

public class King : BasePiece
{
    //private Rook mLeftRook = null;
    //private Rook mRightRook = null;
    float timer;
    private float waitTime = 6f;
    bool timerRunning = true;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        mMovement = new Vector3Int(1, 1, 1);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_King");
    }

    public override void Kill()
    {
       
        base.Kill();
        mPieceManager.mIsKingAlive = false;
    }


    /*
    protected override void CheckPathing()
    {

    }

    protected override void Move()
    {

    }

    private bool CanCastle(Rook rook)
    {
        return true;
    }

    private Rook GetRook(int direction, int count)
    {
        return null;
    }
    */
}

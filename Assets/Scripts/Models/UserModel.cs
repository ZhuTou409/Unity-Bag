using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserModel  {
    private int currentExp;
    private int fullExp;
    private int cone;
    private int level;

    public UserModel()
    {
        level = 1;
        cone = 0;
        fullExp = 100;
        currentExp = 0;
    }
    #region 字段封装
    public int CurrentExp
    {
        get
        {
            return currentExp;
        }

        set
        {
            currentExp = value;
        }
    }

    public int FullExp
    {
        get
        {
            return fullExp;
        }

        set
        {
            fullExp = value;
        }
    }

    public int Cone
    {
        get
        {
            return cone;
        }

        set
        {
            cone = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }
    #endregion

}

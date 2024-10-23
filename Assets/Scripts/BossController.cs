using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BossController : AttackingEnemy
{
    [Header("Boss Settings")]
    [SerializeField] private string bossname;
    [Header("Boss Requirements")]
    [SerializeField] private Collider2D col;
    [Header("Achievements")]
    [SerializeField] private Achievement[] relatedAchievements;
    [SerializeField] private bool activated = true;
    protected float passedTime = 0f;

    private new void Start()
    {
        base.Start();
        if (activated) ActivateBossmode();
    }

    protected override void UpdateGUIElement(float value)
    {
        BossmodeGUI.Instance.HP = value;
    }

    public virtual void ActivateBossmode()
    {
        BossmodeGUI.Instance.Toggle(true);
        BossmodeGUI.Instance.Name = bossname;
    }

    protected override void Attack()
    {
        StopAllCoroutines();
        base.Attack();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnDeath()
    {
        BossmodeGUI.Instance.Toggle(false);

        PlayerPrefs.SetInt(GetHashCode().ToString(), 1);

        foreach (Achievement achievement in relatedAchievements)
            if (achievement.validate())
                RootController.Instance.TriggerAchievement(achievement.Title);

        base.OnDeath();
    }

    public override int GetHashCode()
    {
        return this.bossname.GetHashCode();
    }
}

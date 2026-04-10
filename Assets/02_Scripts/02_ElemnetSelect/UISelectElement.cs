using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class UISelectElement : MonoBehaviour
{
    [SerializeField] private List<CardName> cards = new();

    private readonly List<CardName> ruinDeck = new()
    {
        CardName.Shooting,
        CardName.Strike,
        CardName.VileAttack,
        CardName.Assault,
        CardName.Maintenance,
        CardName.Maintenance,
        CardName.Dummy,
        CardName.Ignition,
        CardName.Ignition,
        CardName.MoltenArms,
        CardName.MoltenArms,
        CardName.Ember,
        CardName.Ember,
        CardName.Inferno,
        CardName.Backdraft,
        CardName.BlazeBarrier,
        CardName.Reforge,
        CardName.Reforge,
        CardName.HeatUp,
        CardName.HeatUp,
        CardName.Overheat,    
        CardName.Overheat,
        CardName.Cinder,
        CardName.OilSplash
    };

    private readonly List<CardName> psychicDeck = new()
    {
        CardName.Shooting,
        CardName.Shooting,
        CardName.VileAttack,
        CardName.Assault,
        CardName.Rollout,
        CardName.Rollout,
        CardName.Maintenance,
        CardName.Maintenance,
        CardName.Dummy,
        CardName.GlacierWedge,
        CardName.GlacierWedge,
        CardName.FlowArrow,
        CardName.FlowArrow,
        CardName.EnergyNeedle,
        CardName.EnergyNeedle,
        CardName.Pulse,
        CardName.KineticGrasp,
        CardName.IceShield,
        CardName.IceShield,
        CardName.ElectricField,
        CardName.AccelConcoction,
        CardName.AccelConcoction,
        CardName.SuperConducter,
        CardName.CryoPowder,
        CardName.Disturb
    };

    private readonly List<CardName> combineDeck = new()
    {
        CardName.Shooting,
        CardName.Assault,
        CardName.Maintenance,
        CardName.MoltenArms,
        CardName.GlacierWedge,
        CardName.GlacierWedge,
        CardName.FlowArrow,
        CardName.Ember,
        CardName.Inferno,
        CardName.KineticGrasp,
        CardName.Reforge,
        CardName.Reforge,
        CardName.IceShield,
        CardName.IceShield,
        CardName.Overheat,
        CardName.Overheat,
        CardName.Cinder,
        CardName.AccelConcoction,
        CardName.AccelConcoction,
        CardName.Disturb
    };

    public void OnClickRuin()
    {
        DeckManager.Instance.SelectedElement = ElementType.Ruin;
        DeckManager.Instance.AddCards(ruinDeck);

        SceneManager.LoadScene(SceneName.Map);
        SoundManager.Instance.PlayMapSound();
    }

    // 2. Ice 속성 선택 버튼 연결
    public void OnClickPsychic()
    {
        DeckManager.Instance.SelectedElement = ElementType.Psychic;
        DeckManager.Instance.AddCards(psychicDeck);

        SceneManager.LoadScene(SceneName.Map);
        SoundManager.Instance.PlayMapSound();
    }

    // 3. Grass 속성 선택 버튼 연결
    public void OnClickCombine()
    {
        DeckManager.Instance.SelectedElement = ElementType.Psychic;
        DeckManager.Instance.AddCards(combineDeck);

        SceneManager.LoadScene(SceneName.Map);
        SoundManager.Instance.PlayMapSound();
    }
}
using UnityEngine;
using Zenject;

public class SightChangeInstaller : MonoInstaller
{
    [SerializeField] GameObject sightChangeObj;
    public override void InstallBindings()
    {
        Container
            .Bind<IChangeSightColor>() // InjectアトリビュートがついているIChangeSightColor型のフィールドに
            .To<TarretScreenSliderChanger>()
            .FromComponentOn(sightChangeObj) // TarretScreenSliderChangerクラスのインスタンスを注入する
            .AsTransient();
    }
}
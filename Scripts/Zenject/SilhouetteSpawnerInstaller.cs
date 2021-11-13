using UnityEngine;
using Zenject;

public class SilhouetteSpawnerInstaller : MonoInstaller
{
    [SerializeField] GameObject spawner;
    public override void InstallBindings()
    {
        Container
            .Bind<ISpawnable>() // InjectアトリビュートがついているIChangeSightColor型のフィールドに
            .To<SilhouetteActivateManager>()
            .FromComponentOn(spawner) // TarretScreenSliderChangerクラスのインスタンスを注入する
            .AsTransient();
    }
}
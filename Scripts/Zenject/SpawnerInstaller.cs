using UnityEngine;
using Zenject;

public class SpawnerInstaller : MonoInstaller
{
    [SerializeField] GameObject spawner;
    public override void InstallBindings()
    {
        Container
            .Bind<ISpawnable>() // InjectアトリビュートがついているIChangeSightColor型のフィールドに
            .To<BlockSpawnerManager>()
            .FromComponentOn(spawner) // TarretScreenSliderChangerクラスのインスタンスを注入する
            .AsTransient();
    }
}
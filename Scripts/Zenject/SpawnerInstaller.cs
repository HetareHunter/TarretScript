using UnityEngine;
using Zenject;

public class SpawnerInstaller : MonoInstaller
{
    [SerializeField] GameObject spawner;
    public override void InstallBindings()
    {
        Container
            .Bind<ISpawnable>() // Inject�A�g���r���[�g�����Ă���IChangeSightColor�^�̃t�B�[���h��
            .To<BlockSpawnerManager>()
            .FromComponentOn(spawner) // TarretScreenSliderChanger�N���X�̃C���X�^���X�𒍓�����
            .AsTransient();
    }
}
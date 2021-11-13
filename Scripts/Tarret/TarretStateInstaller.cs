using UnityEngine;
using Zenject;
using Tarret;

public class TarretStateInstaller : MonoInstaller
{
    [SerializeField] GameObject tarret;
    public override void InstallBindings()
    {
        Container
            .Bind<ITarretStateChangeable>() // Inject�A�g���r���[�g�����Ă���IChangeSightColor�^�̃t�B�[���h��
            .To<TarretStateManager>()
            .FromComponentOn(tarret) // TarretScreenSliderChanger�N���X�̃C���X�^���X�𒍓�����
            .AsTransient();
    }
}
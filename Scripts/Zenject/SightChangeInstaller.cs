using UnityEngine;
using Zenject;

public class SightChangeInstaller : MonoInstaller
{
    [SerializeField] GameObject sightChangeObj;
    public override void InstallBindings()
    {
        Container
            .Bind<IChangeSightColor>() // Inject�A�g���r���[�g�����Ă���IChangeSightColor�^�̃t�B�[���h��
            .To<TarretScreenSliderChanger>()
            .FromComponentOn(sightChangeObj) // TarretScreenSliderChanger�N���X�̃C���X�^���X�𒍓�����
            .AsTransient();
    }
}
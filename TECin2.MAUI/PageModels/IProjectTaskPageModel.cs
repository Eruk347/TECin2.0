using CommunityToolkit.Mvvm.Input;
using TECin2.MAUI.Models;

namespace TECin2.MAUI.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}
using System;
using TarefasApp.Models;

namespace TarefasApp
{
    public partial class TaskDetailsPage : ContentPage
    {
        public event EventHandler<TodoTask> TaskAdded;
        public event EventHandler<TodoTask> TaskUpdated;
        public event EventHandler<TodoTask> TaskDeleted;

        public TodoTask CurrentTask { get; set; }

        public TaskDetailsPage(TodoTask task = null)
        {
            InitializeComponent();
            CurrentTask = task ?? new TodoTask();
            BindingContext = CurrentTask;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (CurrentTask.Id == 0)
            {
                // Novo item, então dispara o evento TaskAdded
                TaskAdded?.Invoke(this, CurrentTask);
            }
            else
            {
                // Item existente, dispara o evento TaskUpdated
                TaskUpdated?.Invoke(this, CurrentTask);
            }

            await Navigation.PopAsync();
        }
        private async void OnEditClicked(object sender, EventArgs e)
        {
            // Abre a página de edição da tarefa passando a tarefa atual como parâmetro
            var editPage = new AddEditTaskPage(CurrentTask);
            editPage.TaskUpdated += (s, updatedTask) =>
            {
                CurrentTask = updatedTask;
                BindingContext = CurrentTask; // Atualiza o BindingContext para refletir as mudanças na tela
                TaskUpdated?.Invoke(this, updatedTask);
            };
            await Navigation.PushAsync(editPage);
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Excluir Tarefa", "Deseja realmente excluir esta tarefa?", "Sim", "Não");
            if (confirm)
            {
                TaskDeleted?.Invoke(this, CurrentTask);
                await Navigation.PopAsync();
            }
        }
    }
}

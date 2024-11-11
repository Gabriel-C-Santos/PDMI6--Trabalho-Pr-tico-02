using System;
using TarefasApp.Models;

namespace TarefasApp
{
    public partial class AddEditTaskPage : ContentPage
    {
        private TodoTask _task;

        // Definição dos eventos
        public event EventHandler<TodoTask> TaskAdded;
        public event EventHandler<TodoTask> TaskUpdated;
        public event EventHandler<TodoTask> TaskDeleted;

        public AddEditTaskPage(TodoTask task = null)
        {
            InitializeComponent();
            _task = task ?? new TodoTask();

            TitleEntry.Text = _task.Title;
            DescriptionEditor.Text = _task.Description;
            TaskDatePicker.Date = _task.DueDate == default ? DateTime.Now : _task.DueDate; // Data padrão como hoje se estiver nulo
            PriorityPicker.SelectedItem = _task.Priority; // Seleciona a prioridade atual

            if (task != null)
            {
                SaveButton.Text = "Salvar";
                DeleteButton.IsVisible = true;
            }
            else
            {
                DeleteButton.IsVisible = false;
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            _task.Title = TitleEntry.Text;
            _task.Description = DescriptionEditor.Text;
            _task.DueDate = TaskDatePicker.Date;
            string selectedPriority = PriorityPicker.SelectedItem.ToString();
            switch (selectedPriority)
            {
                case "Alta":
                    _task.Priority = 3;
                    break;
                case "Média":
                    _task.Priority = 2;
                    break;
                case "Baixa":
                    _task.Priority = 1;
                    break;
                default:
                    _task.Priority = 0; // Valor padrão, caso não haja correspondência
                    break;
            }

            if (_task.Id == 0)
            {
                // Dispara o evento TaskAdded para uma nova tarefa
                TaskAdded?.Invoke(this, _task);
            }
            else
            {
                // Dispara o evento TaskUpdated para uma tarefa existente
                TaskUpdated?.Invoke(this, _task);
            }

            await Navigation.PopAsync();
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            // Dispara o evento TaskDeleted para exclusão da tarefa
            TaskDeleted?.Invoke(this, _task);

            await Navigation.PopAsync();
        }
    }
}

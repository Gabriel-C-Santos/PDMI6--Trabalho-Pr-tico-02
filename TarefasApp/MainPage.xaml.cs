using TarefasApp.Models;
using System.Linq;

namespace TarefasApp
{
    public partial class MainPage : ContentPage
    {
        private List<TodoTask> tasks;

        public MainPage()
        {
            InitializeComponent();
            tasks = new List<TodoTask>();
            TasksCollectionView.ItemsSource = tasks;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateTaskList();
        }

        private void UpdateTaskList()
        {
            // Ordena as tarefas por data de entrega e, em caso de empate, pela prioridade
            var orderedTasks = tasks
                .OrderBy(task => task.DueDate)
                .ThenBy(task => task.Priority)
                .ToList();

            TasksCollectionView.ItemsSource = null; // Limpa a CollectionView
            TasksCollectionView.ItemsSource = orderedTasks; // Atualiza com a lista ordenada
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            var addEditPage = new AddEditTaskPage();
            addEditPage.TaskAdded += (s, newTask) =>
            {
                tasks.Add(newTask);
                UpdateTaskList(); // Atualiza a lista para aplicar a ordenação
            };
            await Navigation.PushAsync(addEditPage);
        }

        private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is TodoTask selectedTask)
            {
                var addEditPage = new AddEditTaskPage(selectedTask);
                addEditPage.TaskUpdated += (s, updatedTask) =>
                {
                    int index = tasks.IndexOf(selectedTask);
                    if (index >= 0)
                    {
                        tasks[index] = updatedTask;
                        UpdateTaskList(); // Atualiza a lista para aplicar a ordenação
                    }
                };
                addEditPage.TaskDeleted += (s, deletedTask) =>
                {
                    tasks.Remove(deletedTask);
                    UpdateTaskList(); // Atualiza a lista para aplicar a ordenação
                };
                await Navigation.PushAsync(addEditPage);
                TasksCollectionView.SelectedItem = null;
            }
        }
    }
}

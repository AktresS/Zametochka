using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.Json;


namespace Zametochka {
    internal class TaskViewModel : INotifyPropertyChanged {

        private ObservableCollection<Task> _tasks = new ObservableCollection<Task>();


        public ObservableCollection<Task> Tasks {
            get => _tasks;
            set {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        private string _addedTask;
        public string AddedTask {
            get => _addedTask;
            set {
                _addedTask = value;
                OnPropertyChanged(nameof(AddedTask));
            }
        }

        public ICommand AddTask { get; }
        public ICommand OneAddTask { get; }

        public ICommand DeleteTask { get; }


        private bool _isActive;

        public bool IsActive {
            get => _isActive;
            set {
                _isActive = value;
                OnPropertyChanged(nameof(_isActive));
            }
        }

        public TaskViewModel() {
            LoadTasks();



            AddTask = new Command(
                () => { 
                    Tasks.Add(new Task { Text = AddedTask, IsDone = false }); 
                    AddedTask = ""; 
                    SaveTasks(); 
                },

                () => string.IsNullOrWhiteSpace(AddedTask) == false);
            
            

            
            DeleteTask = new Command<Task>(
                task => {
                    Tasks.Remove(task);
                    SaveTasks();
                });
        }

        private void SaveTasks() {
            string json = JsonSerializer.Serialize(Tasks);
            Preferences.Set("Tasks", json);
        }

        private void LoadTasks() {
            string json = Preferences.Get("Tasks", "");
            if (!string.IsNullOrWhiteSpace(json)) {
                Tasks = JsonSerializer.Deserialize<ObservableCollection<Task>>(json);

            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    class Task : INotifyPropertyChanged {
        private bool _isDone;
        private string _text;
        


        public bool IsDone {
            get => _isDone;
            set {
                _isDone = value;
                OnPropertyChanged(nameof(IsDone));
            }
        }

        public string Text {
            get => _text;
            set {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DalliKlick.Implementations.Interfaces;
using DalliKlick.Implementations.Interfaces.Services;
using DalliKlick.Implementations.Window;
using DalliKlick.Views.Game;
using DevExpress.Mvvm;

namespace DalliKlick.Views.Menu
{
    public sealed class MenuViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IGameImageService _gameImageService;
        private readonly IMessageManager _messageManager;
        private List<GameItem> _gameItems;
        private GameItem _currentGameItem;
        private GameViewModel _gameViewModel;
        private int _gameItemsDisplayCount;
        private int _currentGameItemDisplayIndex;

        public MenuViewModel(IWindowManager windowManager, IGameImageService gameImageService, IMessageManager messageManager)
        {
            _windowManager = windowManager;
            _gameImageService = gameImageService;
            _messageManager = messageManager;
            NextCommand = new DelegateCommand(Next, CanNext);
            BackCommand = new DelegateCommand(Back, CanBack);
            DisplayName = "Admin";
        }

        public void LoadImages()
        {
            var imagePaths = _gameImageService.GetImagePaths();
            if (imagePaths.Length == 0)
            {
                _messageManager.ShowError(
                    "Es wurden keine Bilder gefunden.\nBitte legen Sie Ihre Bilder in das Bilder Verzeichnis und bestätigen Sie anschließend mit OK.");
                LoadImages();
                return;
            }

            _gameViewModel = IoC.Get<GameViewModel>();
            _windowManager.ShowWindow(_gameViewModel, settings: WindowSettings.With().FixedSize(1024, 800));

            GameItems = imagePaths.Select(x => new GameItem(x)).ToList();
            CurrentGameItem = GameItems.First();
        }

        public int GameItemsDisplayCount
        {
            get => _gameItemsDisplayCount;
            private set
            {
                if (value == _gameItemsDisplayCount) return;
                _gameItemsDisplayCount = value;
                NotifyOfPropertyChange();
            }
        }

        public int CurrentGameItemDisplayIndex
        {
            get => _currentGameItemDisplayIndex;
            private set
            {
                if (value == _currentGameItemDisplayIndex) return;
                _currentGameItemDisplayIndex = value;
                NotifyOfPropertyChange();
            }
        }

        public List<GameItem> GameItems
        {
            get => _gameItems;
            private set
            {
                if (Equals(value, _gameItems)) return;
                _gameItems = value;
                GameItemsDisplayCount = value?.Count ?? -1 + 1;
                NotifyOfPropertyChange();
            }
        }

        public GameItem PreviousGameItem => CanBack() ? GameItems[GameItems.IndexOf(CurrentGameItem)-1] : null;
        public GameItem NextGameItem => CanNext() ? GameItems[GameItems.IndexOf(CurrentGameItem) + 1] : null;
        public GameItem CurrentGameItem
        {
            get => _currentGameItem;
            set
            {
                if (Equals(value, _currentGameItem)) return;
                _currentGameItem = value;
                NotifyOfPropertyChange();
                if (value != null)
                {
                    CurrentGameItemDisplayIndex = GameItems.IndexOf(value) + 1;
                    NotifyOfPropertyChange(() => PreviousGameItem);
                    NotifyOfPropertyChange(() => NextGameItem);
                    _gameViewModel.SetGameItem(value);
                }
            }
        }

        private bool CanBack()
        {
            if (_gameViewModel == null || GameItems?.IndexOf(CurrentGameItem) <= 0)
                return false;
            return true;
        }

        private void Back()
        {
            if(!CanBack())
                return;
            CurrentGameItem = GameItems[GameItems.IndexOf(CurrentGameItem) - 1];
        }

        private bool CanNext()
        {
            if (_gameViewModel == null || GameItems?.IndexOf(CurrentGameItem)+1 > GameItems?.Count -1)
                return false;
            return true;
        }

        private void Next()
        {
            if(!CanNext())
                return;
            CurrentGameItem = GameItems[GameItems.IndexOf(CurrentGameItem) + 1];
        }

        public ICommand NextCommand { get; }
        public ICommand BackCommand { get; }
    }
}
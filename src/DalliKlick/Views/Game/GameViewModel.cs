using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using DalliKlick.Annotations;
using DalliKlick.Views.Game.Overlays;

namespace DalliKlick.Views.Game
{
    public sealed class GameViewModel : Screen
    {
        private GameItem _currentGame;
        private IOverlay _overlay;

        public GameViewModel()
        {
            DisplayName = "Spiel";
        }

        public override void CanClose(Action<bool> callback)
        {
            callback.Invoke(false);
        }

        public void SetGameItem(GameItem gameItem)
        {
            CurrentGame = gameItem ?? throw new ArgumentNullException(nameof(gameItem));
            Overlay = null;
            Overlay = new Puzzle();
        }

        public IOverlay Overlay
        {
            get => _overlay;
            set
            {
                _overlay = value;
                NotifyOfPropertyChange();
            }
        }

        public GameItem CurrentGame
        {
            get => _currentGame;
            private set
            {
                if (Equals(value, _currentGame)) return;
                _currentGame = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
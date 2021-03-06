﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using Minesweeper.Models;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Minesweeper.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */


        private Model model;
        public Model Model
        {
            get
            {
                if (model == null) model = new Model();
                return model;
            }
        }          

        public int Width
        {
            get
            {
                return Model.Width;  
            }
        }
        public int Height
        {
            get
            {
                return Model.Height;
            }
        }

        public int AppWidth
        {
            get
            {
                return 30 * (int)Width;
            }
        }
        public int AppHeight
        {
            get
            {
                return 30 * (int)Height;
            }
        }

        private ObservableSynchronizedCollection<CellModel> cells;
        public ObservableSynchronizedCollection<CellModel> Cells
        {
            get
            {
                if (cells == null) cells = new ObservableSynchronizedCollection<CellModel>();
                return cells;
            }
            set
            {
                this.cells = value;
                RaisePropertyChanged("Cells");
            }
        }

        public class CellModel
        {
            public int ColumnIndex;
            public int RowIndex;
            public bool IsVisible;
            public bool IsMine;
            public bool IsFlag;
            public int NumberAdjacentMines;
            private string name = string.Empty;
            public string Name
            {
                get
                {
                    if (IsFlag) name = "🚩";
                    if (NumberAdjacentMines > 0) name = NumberAdjacentMines.ToString();
                    return name;
                }
            }
            public string pos
            {
                get
                {
                    return RowIndex.ToString() + "," + ColumnIndex.ToString();
                }
            }
        }

        public void Initialize()
        {
            this.Cells = new ObservableSynchronizedCollection<CellModel>();
            var rowIdx = 0;
            var items = this.Model.GetAllGridData2d();
            foreach (var row in items)
            {
                var colIdx = 0;
                foreach(var item in row)
                {
                    bool isVisible = true;
                    if (item == -1 || item == -2) isVisible = false;
                    bool isMine = false;
                    if (item == -3) isMine = true;
                    bool isFlag = false;
                    if (item == -2) isFlag = true;
                    int num = item;
                     
                    Cells.Add(new CellModel() { ColumnIndex = colIdx, RowIndex = rowIdx, IsMine=isMine, IsFlag=isFlag, IsVisible=isVisible, NumberAdjacentMines=num });
                    colIdx++;
                }
                rowIdx++;
            }                   
        }


        #region OpenCommand
        private ListenerCommand<string> openCommand;

        public ICommand OpenCommand
        {
            get
            {
                if (openCommand == null)
                {
                    openCommand = new ListenerCommand<string>(Open);  
                }
                return openCommand;
            }
        }

        private void Open(string pos)
        {

            Regex r = new Regex(",");
            var m = r.Split(pos);
            this.model.Challenge(Int32.Parse(m.First()), Int32.Parse(m.Last()));
        }
        #endregion





    }
}

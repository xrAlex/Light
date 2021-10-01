using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Light.Views.Tray
{
    public partial class TrayMenuView
    {
        public TrayMenuView()
        {
            InitializeComponent();
        }

        private void OnButtonMouserEnterLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Получение источника события и используемого ею ресурса.
            Button button = (Button)sender;
            Brush brush = button.Background;

            // Определение какое было событие. Используется для установки значения анимации
            bool isMouseEnter = e.RoutedEvent == MouseEnterEvent;

            // Создание и запуск анимации.
            DoubleAnimation animation = new DoubleAnimation()
            {
                To = isMouseEnter ? 1 : 0,
                Duration = TimeSpan.FromMilliseconds(300)
            };
            brush.BeginAnimation(Brush.OpacityProperty, animation);
        }

        /*
        <ControlTemplate.Triggers>
            <DataTrigger Binding="{Binding IsMouseOver, ElementName=Border}" Value="True">
                <DataTrigger.EnterActions>
                    <BeginStoryboard>
                        <Storyboard>
                            <DoubleAnimation To="1" Duration="0:0:0.3" Storyboard.TargetName="Border" Storyboard.TargetProperty="Background.(SolidColorBrush.Opacity)"/>
                        </Storyboard>
                    </BeginStoryboard>
                </DataTrigger.EnterActions>
                <DataTrigger.ExitActions>
                    <BeginStoryboard>
                        <Storyboard>
                            <DoubleAnimation To="0" Duration="0:0:0.3" Storyboard.TargetName="Border" Storyboard.TargetProperty="Background.(SolidColorBrush.Opacity)"/>
                        </Storyboard>
                    </BeginStoryboard>
                </DataTrigger.ExitActions>
            </DataTrigger>
        </ControlTemplate.Triggers>
        */
    }
}
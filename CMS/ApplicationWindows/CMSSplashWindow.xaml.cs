using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace CMS
{
    /// <summary>
    /// Interaction logic for CMSSplashWindow.xaml
    /// </summary>
    public partial class CMSSplashWindow : Window
    {
        private bool _isAnimationComplete { get; set; }

        public CMSSplashWindow()
        {
            InitializeComponent();
        }

        //private void Storyboard_Completed(object? sender, EventArgs e)
        //{
        //    if(_isAnimationComplete && Opacity == 0) 
        //        Close();
        //}

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    if (!_isAnimationComplete) 
        //        e.Cancel = true;

        //    Storyboard storyboard = new Storyboard();
        //    DoubleAnimation animation = AnimationHelpers.DoubleAnimation(0.8, 1, 0, AnimationEasingKind.QuinticEase, EasingMode.EaseInOut, 0);
        //    storyboard.Children.Add(animation);
        //    storyboard.Completed += Storyboard_Completed;
        //    Storyboard.SetTargetProperty(storyboard, new PropertyPath("Opacity"));
        //    storyboard.Begin(this);
        //    _isAnimationComplete = true;
        //    base.OnClosing(e);
        //}
    }
}

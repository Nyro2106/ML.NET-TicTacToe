using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MNIST_Demo.Views
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class TicTacToePage : Page
  {
    public InkCanvas CurrentCanvas { get; set; }
    
    static bool isZeroTurn = true;

    static int[,] winConditions = new int[8, 3]
    {
      {0,1,2}, {3,4,5}, {6,7,8},
      {0,3,6}, {1,4,7}, {2,5,8},
      {0,4,8}, {2,4,6}
    };

    static int[] currentGrid;

    public TicTacToePage()
    {
      this.InitializeComponent();
      this.InitializeGrid();
      this.InitializeInkCanvases();
    }

    private void InitializeInkCanvases()
    {
      foreach (var child in this.UxGameGrid.Children)
      {
        if (child is InkCanvas canvas)
        {
          canvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen | Windows.UI.Core.CoreInputDeviceTypes.Touch;
          canvas.InkPresenter.UpdateDefaultDrawingAttributes(
              new Windows.UI.Input.Inking.InkDrawingAttributes()
              {
                Color = Windows.UI.Colors.Black,
                Size = new Size(22, 22),
                IgnorePressure = true,
                IgnoreTilt = true,
              });
        }
      }
    }

    private void InitializeGrid()
    {
      currentGrid = new int[9];
      for (int i = 0; i < 9; i++)
      {
        currentGrid[i] = -1;
      }
    }

    private void UxClearAppBarButton_Click(object sender, RoutedEventArgs e)
    {
      foreach (var child in UxGameGrid.Children)
      {
        if (child is Button button)
        {
          button.Content = string.Empty;
        }
      }
      this.InitializeGrid();
      this.UxStatusTextBlock.FontWeight = Windows.UI.Text.FontWeights.Normal;
      this.UxStatusTextBlock.Text = "Spieler '0' ist dran";
    }

    private async void EvaluateButton(Button button, int index)
    {
      DrawingContentDialog dialog = new DrawingContentDialog();
      await dialog.ShowAsync();
      if (dialog.Result != -1)
      {
        button.Content = dialog.Result.ToString();
        isZeroTurn = !isZeroTurn;
        SetHeaderLabel(isZeroTurn);
        currentGrid[index] = dialog.Result;
        CheckFields(dialog.Result);
      }
      else
      {
        button.Content = string.Empty;
      }
    }

    private void SetHeaderLabel(bool isZeroTurn)
    {
      if (isZeroTurn)
      {
        this.UxStatusTextBlock.Text = "Spieler '0' ist dran";
      }
      else
      {
        this.UxStatusTextBlock.Text = "Spieler '1' ist dran";
      }
    }

    private void CheckFields(int currentNumber)
    {
      for (int i = 0; i < 8; i++)
      {
        if (currentGrid[winConditions[i, 0]] == currentNumber &&
          currentGrid[winConditions[i, 1]] == currentNumber &&
          currentGrid[winConditions[i, 2]] == currentNumber 
          )
        {
          this.UxStatusTextBlock.FontWeight = Windows.UI.Text.FontWeights.Bold;
          this.UxStatusTextBlock.Text = $"Spieler '{currentNumber}' gewinnt!";
        }
      }
    }

    private void UxGrid00Button_Click(object sender, RoutedEventArgs e)
    {
      EvaluateButton((Button)sender, 0);
    }

    private void UxGrid20Button_Click(object sender, RoutedEventArgs e)
    {
      EvaluateButton((Button)sender, 1);
    }

    private void UxGrid40Button_Click(object sender, RoutedEventArgs e)
    {
      EvaluateButton((Button)sender, 2);
    }

    private void UxGrid02Button_Click(object sender, RoutedEventArgs e)
    {
      EvaluateButton((Button)sender, 3);
    }

    private void UxGrid22Button_Click(object sender, RoutedEventArgs e)
    {
      EvaluateButton((Button)sender, 4);
    }

    private void UxGrid42Button_Click(object sender, RoutedEventArgs e)
    {
      EvaluateButton((Button)sender, 5);
    }

    private void UxGrid04Button_Click(object sender, RoutedEventArgs e)
    {
      EvaluateButton((Button)sender, 6);
    }

    private void UxGrid24Button_Click(object sender, RoutedEventArgs e)
    {
      EvaluateButton((Button)sender, 7);
    }

    private void UxGrid44Button_Click(object sender, RoutedEventArgs e)
    {
      EvaluateButton((Button)sender, 8);
    }

    private void UxBackButton_Click(object sender, RoutedEventArgs e)
    {
      this.Frame.Navigate(typeof(MainPage));
    }
  }

}

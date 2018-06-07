using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MNIST_Demo.Views
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class TicTacToePage : Page
  {

    static int[,] windConditions = new int[8, 3]
    {
      {0,1,2}, {3,4,5}, {6,7,8},
      {0,3,6}, {1,4,7}, {2,5,8},
      {0,4,8}, {2,4,6}
    };

    static int[] currentGrid = new int[9];

    public TicTacToePage()
    {
      this.InitializeComponent();
      this.InitializeGrid();
    }

    private void InitializeGrid()
    {
      for (int i = 0; i < 9; i++)
      {
        currentGrid[i] = -1;
      }
    }

    private void UxClearAppBarButton_Click(object sender, RoutedEventArgs e)
    {
      foreach (var canvas in UxGameGrid.Children)
      {
        if (canvas is InkCanvas inkCanvas)
        {
          inkCanvas.InkPresenter.StrokeContainer.Clear();
        }
      }
    }

    private void UxNextPlayerButton_Click(object sender, RoutedEventArgs e)
    {
      this.UxStatusTextBlock.Text = "Spieler 2";
    }

    private async void EvaluateButton(Button button, int index)
    {
      DrawingContentDialog dialog = new DrawingContentDialog();
      await dialog.ShowAsync();
      if (dialog.Result != -1)
      {
        button.Content = dialog.Result.ToString();
        currentGrid[index] = dialog.Result;
        CheckFields(dialog.Result);
      }
      else
      {
        button.Content = string.Empty;
      }
    }

    private void CheckFields(int currentNumber)
    {
      for (int i = 0; i < 8; i++)
      {
        if (currentGrid[windConditions[i, 0]] == currentNumber &&
          currentGrid[windConditions[i, 0]] == currentNumber &&
          currentGrid[windConditions[i, 0]] == currentNumber 
          )
        {
          this.UxStatusTextBlock.Text = "Gewonnen";
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
  }

}

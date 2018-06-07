﻿using System;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MNIST_Demo.Views
{
  public sealed partial class DrawingContentDialog : ContentDialog
  {
    private Helper helper = new Helper();

    private MNISTModel ModelGen = new MNISTModel();
    private MNISTModelInput ModelInput = new MNISTModelInput();
    private MNISTModelOutput ModelOutput = new MNISTModelOutput();
    public Image CurrentDrawing { get; set; } = new Image();
    internal int Result = -1;

    private RenderTargetBitmap renderBitmap = new RenderTargetBitmap();

    public DrawingContentDialog()
    {
      this.InitializeComponent();
      this.UxInkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen | Windows.UI.Core.CoreInputDeviceTypes.Touch;
      this.UxInkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(
          new Windows.UI.Input.Inking.InkDrawingAttributes()
          {
            Color = Windows.UI.Colors.White,
            Size = new Size(22, 22),
            IgnorePressure = true,
            IgnoreTilt = true,
          }
      );
      LoadModel();
    }

    private async void LoadModel()
    {
      //Load a machine learning model
      StorageFile modelFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/MNIST.onnx"));
      ModelGen = await MNISTModel.CreateMNISTModel(modelFile);
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {

    }

    private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
      this.Result = -1;
    }

    private void UxConfirmButton_Click(object sender, RoutedEventArgs e)
    {
      EvaluateDrawing();      
    }

    private async void EvaluateDrawing()
    {
      //Bind model input with contents from InkCanvas      
      ModelInput.Input3 = await helper.GetHandWrittenImage(this.UxInkGrid);

      //Evaluate the model
      ModelOutput = await ModelGen.EvaluateAsync(ModelInput);

      //Iterate through evaluation output to determine highest probability digit
      float maxProb = 0;
      int maxIndex = 0;
      for (int i = 0; i < 10; i++)
      {
        if (ModelOutput.Plus214_Output_0[i] > maxProb)
        {
          maxIndex = i;
          maxProb = ModelOutput.Plus214_Output_0[i];
        }
      }
      this.Result = maxIndex;

      switch (Result)
      {
        case 0:
          this.UxTestTextBlock.Text = $"0 erkannt";
          break;
        case 1:
          this.UxTestTextBlock.Text = $"1 erkannt";
          break;
        default:
          this.UxTestTextBlock.Text = "Nicht erkannt, bitte nochmal versuchen";
          this.UxInkCanvas.InkPresenter.StrokeContainer.Clear();
          this.Result = -1;
          break;
      }
      //this.UxTestTextBlock.Text = Result.ToString();
    }

    private void UxClearButton_Click(object sender, RoutedEventArgs e)
    {
      this.UxInkCanvas.InkPresenter.StrokeContainer.Clear();
      this.Result = -1;
    }
  }
}
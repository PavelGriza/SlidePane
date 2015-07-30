SlidePane
===

Simple and lightweight control for <a href="http://msdn.microsoft.com/en-us/library/windows/apps/dn609832.aspx" target="_blank">Windows Universal Applications</a> which allow to slide between one or two panels. It's compatible for Win app and WP8.1 app.
You can locate 2 additional content panes at the left and right sides from your main container of a page and using slide left / slide right gestures or specific buttons for opening and closing additional panes.

How it works
---
In SladePaneControl library you can find SlidePane and ContentPane.
SlidePane has 2 properties: LeftPane and RightPane which may or may not be initialize by ContentPane instances.
ContentPane contains ButtonVisibility property, which allow you to show or hide slide button.
For mode details look at the Sample of using

Sample of using
---
```c#
<Page x:Class="SlidePane.MainPage"
	//...
	xmlns:сontrol="using:SlidePaneControl">

    <сontrol:SlidePane x:Name="SlidePane">
        <сontrol:SlidePane.LeftPane>
            <сontrol:ContentPane ButtonVisibility="Visible">
                <Grid Width="400" Background="Green">
                    <TextBlock HorizontalAlignment="Center"
                               VerticalAlignment="Center"
                               FontSize="50"
                               Text="Left Pane" />
                </Grid>
            </сontrol:ContentPane>
        </сontrol:SlidePane.LeftPane>

        <Grid Background="Transparent">
            <TextBlock HorizontalAlignment="Center"
                       VerticalAlignment="Center"
                       FontSize="100"
                       Text="Main content" />
        </Grid>

        <сontrol:SlidePane.RightPane>
            <сontrol:ContentPane ButtonVisibility="Visible">
                <Grid Width="500" Background="DarkBlue">
                    <TextBlock HorizontalAlignment="Center"
                               VerticalAlignment="Center"
                               FontSize="50"
                               Text="Right Pane" />
                </Grid>
            </сontrol:ContentPane>
        </сontrol:SlidePane.RightPane>
    </сontrol:SlidePane>
</Page>

```
You can open or close additional pane from code behind:
```c#

private void OnEventHandler(object sender, RoutedEventArgs e)
{
	this.SlidePane.IsLeftPaneOpen = true;
}

```


Install from NuGet
---
> Watch at <a href="https://www.nuget.org/packages/SlidePane/" target="_blank">SlidePane for Universal Apps</a>

License
---
WindowsUniversalLogger is licensed under <a href="http://www.apache.org/licenses/LICENSE-2.0" target="_blank" >License Apache 2.0 License</a>. Refer to license file for more information.

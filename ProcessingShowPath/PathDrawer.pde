class PathDrawer{
  //Variables for matching path to map image
  private int sizeY = 900;
  private int startBiasX = -140;
  private int startBiasZ = -50;
  private float stretchMultiplierX = 2.2;
  private float stretchMultiplierY = 2.2;
  
  //Color variables
  private color forwardColor = color(0,0,255);
  private color backColor = color(255,0,0);
  private color currentColor = forwardColor;
  private int drawAlpha;
  
  //Heatmap drawing options
  private int heatmapCircleDiameter = 5;
  private int heatmapDrawSkip = 5;      //how many frames are skipped when drawing the heatmap. This is used to further reduce the alpha value of the path. 
  private int heatmapDrawCounter = 0;    
  
  //how do we want the path drawer to draw
  private boolean drawForward;
  private boolean drawBack;
  private boolean heatmap;
  
  PathDrawer(boolean drawForward, boolean drawBack, boolean heatmap)
  {
    this.drawForward = drawForward;
    this.drawBack = drawBack;
    this.heatmap = heatmap;
    if(heatmap)
    {
      drawAlpha = 2;
    }
    else
    {
      drawAlpha = 100;
    }
  }
  
  //draw the whole path
  void drawPath(Path path)
  {
    for(int i = 0; i < path.gameplayData.length; i++)
    {
      String coords[] = path.gameplayData[i];
      drawPoint(coords);
    }
  }
  
  //draw a single point in the path
  void drawPoint(String[] coords)
  {
    if(coords.length > 2)    //if coordinates, not mistake and shortcut count
    {
      int x = int(startBiasX + (stretchMultiplierX * Float.parseFloat(coords[0])));
      int z = int(startBiasZ + (stretchMultiplierY * Float.parseFloat(coords[2])));
        
      if(currentColor == forwardColor && drawForward || currentColor == backColor && drawBack)
      {
        if(heatmap)
        {
          heatmapDrawCounter++;
          if(heatmapDrawCounter == heatmapDrawSkip)
          {
            heatmapDrawCounter = 0;
            stroke(currentColor, drawAlpha);
            fill(currentColor, drawAlpha);
            circle(x, sizeY - z, heatmapCircleDiameter);
          }
        }
        else 
        {
          stroke(currentColor, drawAlpha);
          point(x, sizeY - z);
          point(x + 1, sizeY - z);
          point(x + 1, sizeY - z + 1);
          point(x + 1, sizeY - z + 1);
        }
      }
    }
    else if(coords[0].equals("EndTaskCompleted")) //This is the end task tag
    {
      currentColor = backColor;
    }
  }
}

//modify wanted output here
boolean drawForward = true;  //draw the path the participants take to the beehive
boolean drawBack = true;     //draw the path the participants take back from the beehive
boolean drawAll = true;      //draw the paths of all participants
boolean isHeatmap = true;      //draw path as a heatmap
boolean drawArrow = true;    //draw the paths for the Arrow condition
boolean drawNPC = true;      //draw the paths for the NPC condition
String singleFilePath = "arrow/A12.txt";    //path of to the test file when drawing only a single test file

//for drawing single path
Path path;         
PathDrawer drawer;  
int drawIterator;    //used to iterate over the coordinates of the path when drawing single path

void setup() {
  size(1200,900);
  frameRate(1000000000);
  loadPixels();  
  PImage map = loadImage("isometric.png");
  map.filter(GRAY);
  background(map);
  
  if(drawAll)
  {
    drawCondition("arrow", drawArrow);
    drawCondition("bee", drawNPC);
  }
  else
  {
    Reader reader = new Reader();
    path = reader.readPlayerPathFile(singleFilePath);
    drawer = new PathDrawer(drawForward, drawBack, false);  
    PathAnalyser analyser = new PathAnalyser(path);
    analyser.analysePath();
    analyser.printResult(true);
  }
}

void draw() {
  if(!drawAll)
  {
    if(drawIterator < path.pathLength)
    {
      String coords[] = path.gameplayData[drawIterator];
      drawer.drawPoint(coords);
      drawIterator++;
    }
  }
}

void drawCondition(String fileFolderName, boolean drawCondition)
{
  if(drawCondition)
    {
      Reader reader = new Reader();
      reader.getFilesFromFolder("C:/UnityProjects/P5Game/ProcessingShowPath/" + fileFolderName);
      Path[] paths = reader.readPlayerPathFiles(fileFolderName);
     
      println("Drawing " + fileFolderName + " condition files");
      println("participant_number, time_going_forward, time_going_back, mistakes_made, shortcuts_taken, wander_count, wander_time_sum, finished");
      for(int i = 0; i < paths.length; i++)
      {
        PathDrawer drawer = new PathDrawer(drawForward, drawBack, isHeatmap);  
        drawer.drawPath(paths[i]);
        PathAnalyser analyser = new PathAnalyser(paths[i]);
        analyser.analysePath();
        analyser.printResult(false);
      }
    }
}

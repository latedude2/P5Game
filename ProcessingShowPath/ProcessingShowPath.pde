//modify wanted output here
boolean drawForward = true;  //draw the path the participants take to the beehive
boolean drawBack = true;     //draw the path the participants take back from the beehive
boolean drawAll = true;      //draw the paths of all participants
boolean heatmap = true;      //draw path as a heatmap
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
    if(drawArrow)
    {
      Reader arrowReader = new Reader();
      arrowReader.getFilesFromFolder("C:/UnityProjects/P5Game/ProcessingShowPath/arrow");
      Path[] arrowPaths = arrowReader.readGameFiles("arrow");
     
      println("Drawing arrow condition files");
      println("participant_number, time_going_forward, time_going_back, mistakes_made, shortcuts_taken, wander_count, wander_time_sum, finished");
      for(int i = 0; i < arrowPaths.length; i++)
      {
        PathDrawer drawer = new PathDrawer(drawForward, drawBack, heatmap);  
        drawer.drawPath(arrowPaths[i]);
        PathAnalyser analyser = new PathAnalyser(arrowPaths[i]);
        analyser.analysePath();
        analyser.printResult(false);
      }
    }
    
    if(drawNPC)
    {
      Reader npcReader = new Reader();
      npcReader.getFilesFromFolder("C:/UnityProjects/P5Game/ProcessingShowPath/bee");
      Path[] npcPaths = npcReader.readGameFiles("bee");
      
      println("Drawing arrow condition files");
      println("participant_number, time_going_forward, time_going_back, mistakes_made, shortcuts_taken, wander_count, wander_time_sum, finished");
      for(int i = 0; i < npcPaths.length; i++)
      {
        PathDrawer drawer = new PathDrawer(drawForward, drawBack, heatmap); 
        drawer.drawPath(npcPaths[i]);
        PathAnalyser analyser = new PathAnalyser(npcPaths[i]);
        analyser.analysePath();
        analyser.printResult(false);
      }
    }
  }
  else
  {
    Reader reader = new Reader();
    path = reader.readTestFile(singleFilePath);
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

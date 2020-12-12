class PathAnalyser{ 
  private float invalidTimeBecauseAidWasAbandoned = -0.01;    //time set when the aid is abandoned at the end
  
  //forward and back time
  private float forwardTime;      //time taken to reach the beeehive
  private float backTime;         //time taken to go back to the starting area
  private boolean isBackTimeSaved = false;       //has back time been saved
  
  //Breaks taken while playing the game
  private FloatList forwardBreakTimes  = new FloatList();    //amount of time taken for each break when going to the beehive
  private FloatList backBreakTimes  = new FloatList();       //amount of time taken for each break when going back from the beehive
  private float noMovementTimeForward = 0;      //cumulative time taken on breaks when going forward
  private float noMovementTimeBack = 0;         //cumulative time taken on breaks when going back
  private int breakCount = 0;                   //amount of breaks taken overall
  private float minimumBreakTime = 5.0;         //minimum time of no movement for it to be considered a break

  //time wandering away from the aid
  private FloatList returnToAidTimes = new FloatList();    //times at which the player returned to the aid
  private FloatList wanderToAidTimes  = new FloatList();   //times at which the player left the aid
  private float wanderTimeSum;                             //sum of times when the player is away from the aid
  
  private int mistakes;  //amount of mistakes made when going back
  private int shortcuts; //amount of shortcuts taken when going back
  
  Path path;
  
  PathAnalyser(Path path)
  {
    this.path = path;
  }
  
  // analyse a single path
  void analysePath()
  {
    findTimes();
    FloatList wanderTimes = calculateTimesAwayFromBee(returnToAidTimes, wanderToAidTimes);
    wanderTimeSum = floatListSum(wanderTimes);
    noMovementTimeForward = floatListSum(forwardBreakTimes);
    noMovementTimeBack = floatListSum(backBreakTimes);
  }
  
  // print the result of the analysis
  void printResult(boolean printBreakTimes)
  {
    String finishedString;
    if (wanderToAidTimes.size() != 0 && wanderToAidTimes.get(0) == invalidTimeBecauseAidWasAbandoned)
    {
        finishedString = "Left aid";
    }
    else if(isBackTimeSaved)
    {
      finishedString = "Yes";
    }
    else 
    {
      finishedString = "No";
    }
    forwardTime -= noMovementTimeForward;  //Adjust for participants taking breaks
    backTime -= noMovementTimeBack;
    println(path.participantNumber + ", " + forwardTime + ", " + backTime + ", " + mistakes + ", " + shortcuts + ", " + wanderToAidTimes.size() + ", " + wanderTimeSum  + ", " + finishedString);
    if(printBreakTimes)
    {
      println("Breaks taken: " + breakCount + " Standing still time forward: " + noMovementTimeForward + " Standing still time back: " + noMovementTimeBack);
    }
  }
  
  // find all the time based variables - forwardTime, backTime, isBackTimeSaved
  private void findTimes()
  {
    String[] previousCoords = null;        //coordinates from the previous point in the path
    float noMovementTime = 0;              //for how long the player has not moved
    boolean isTakingBreak = false;         //is the player currently standing still     
    float firstFrameTime = -1.0;           //when did the first frame of gameplay happen
    float frameTime = 0;                   //time of the current frame
    boolean isForwardTimeSaved = false;    //has forward time been saved
    for(int i = 0; i < path.gameplayData.length; i++)
    {
      String coords[] = path.gameplayData[i];
      
      switch(coords.length) 
      {
        case 1:   //line with tag
          switch(coords[0]) 
          {
            case "EndTaskCompleted":
              isForwardTimeSaved = true;
              break;
            case "ReturnedToBee":
              returnToAidTimes.append(frameTime);
              break;
            case "WanderedFromBee":
              wanderToAidTimes.append(frameTime);
              break;
            case "TestCompleted":
              isBackTimeSaved = true;
              break;
          }
          break;
        case 2:    //line with player mistake and shortcut counts
          mistakes = Integer.parseInt(coords[0]);
          shortcuts = Integer.parseInt(coords[1]);
          break;
          
        case 8:    //line with coordinates
          if(previousCoords != null)
          {
            if(hasPlayerStopped(previousCoords, coords))
            {
              isTakingBreak = true;
              noMovementTime += Float.parseFloat(coords[7]) - Float.parseFloat(previousCoords[7]);
            }
            else
            {
              if(noMovementTime > minimumBreakTime)  //We need to check against a bigger time than one frame to make sure the break is player behavior and not a low framerate
              {
                breakCount++;
                if(!isForwardTimeSaved)
                {
                  forwardBreakTimes.append(noMovementTime);
                }
                else
                {
                  backBreakTimes.append(noMovementTime);
                }
              }
              isTakingBreak = false;
              noMovementTime = 0;
            }
          }
          previousCoords = coords;
          frameTime = Float.parseFloat(coords[7]);
          
          adjustForStartingTime(firstFrameTime, frameTime);
          
          if(!isForwardTimeSaved)
          {
             forwardTime = frameTime;
          }
          if(!isBackTimeSaved)
          {
             backTime = frameTime - forwardTime;
          }
          break;
        default:           
          break;
      }
    } 
  }
  private FloatList calculateTimesAwayFromBee(FloatList returnTimes, FloatList awayTimes)
  {
    FloatList times = new FloatList();    //how long is each wander
    int timesToCheck = awayTimes.size();  //how many times should be found
    if(returnTimes.size() == awayTimes.size())  //if the away and return time count the same, the player abandoned the aid before finishing the beehive task
    {
        times.append(invalidTimeBecauseAidWasAbandoned);
        timesToCheck--;
    }
    for(int i = 0; i < timesToCheck; i++)
    {
      times.append(returnTimes.get(i + 1) - awayTimes.get(i));
    }
    return times;
  }

  private float floatListSum(FloatList times)
  {
    float sum = 0;
    for(int i = 0; i < times.size(); i++)
    {
      sum += times.get(i);
    }
    return sum;
  }
  private boolean hasPlayerStopped(String[] previousCoords, String[] coords)
  {
    for(int i=0; i < coords.length - 1; i++)
    {
      if (!previousCoords[i].equals(coords[i]))
      {
        return false;
      }
    }
    return true;
  }
  
  void adjustForStartingTime(float firstFrameTime, float frameTime)
  {
    //Save time of first frame
    if(firstFrameTime == -1.0)  //if the starting time has not been saved yet
    {
       firstFrameTime = frameTime;
    }
    frameTime -= firstFrameTime;  //Adjust starting time to be 0
  }
}

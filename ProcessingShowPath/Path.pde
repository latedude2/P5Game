class Path{ 
  String[][] gameplayData;    //coordinates and tags from the file
  String participantNumber;   
  int pathLength;             //length of gameplayData, used when drawing single file
  
  Path(String[][] gameplayData, String participantNumber, int pathLength)
  {
    this.gameplayData = gameplayData;
    this.participantNumber = split(participantNumber, "/")[1];
    this.pathLength = pathLength;
  }
} 

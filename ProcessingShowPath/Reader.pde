class Reader{ 
  private String[] filenames;
  static final int maxDataPointCount = 8;
  
  Reader()
  {
    
  }
  
  //get filenames from a specified folder and print them
  void getFilesFromFolder(String fileFolderPath)
  {
    println("Files found: ");
    filenames = listFileNames(fileFolderPath);
    printArray(filenames);
  }
  
  //read all game files from the folder the reader was created with
  Path[] readPlayerPathFiles(String folder)
  {
    Path[] playerPaths = new Path[filenames.length];
    for(int i = 0; i < filenames.length; i++)
    {
        playerPaths[i] = readPlayerPathFile(folder + "/" + filenames[i]);
    }
    return playerPaths;
  }
  
  Path readPlayerPathFile(String filePath)
  {
    BufferedReader reader = createReader(filePath);
    StringList lines = new StringList();
    try
    {
      String line;
      while((line = reader.readLine())!= null)
      {
        line = line.replace(',', '.');
        lines.append(line);
      }
    }
    catch (IOException e)
    {
      e.printStackTrace();
    }
    String[][] gameplayData = new String[lines.size()][maxDataPointCount];
    for(int i = 0; i < lines.size(); i++)
    {
        gameplayData[i] = lines.get(i).split(" ");
    }
    String participantNumber = split(filePath, ".")[0];
    return new Path(gameplayData, participantNumber);
  }
  
  //this function returns all the files in a directory as an array of Strings  
  private String[] listFileNames(String dir) {
    File file = new File(dir);
    if (file.isDirectory()) {
      String names[] = file.list();
      return names;
    } else {
      // If it's not a directory
      return null;
    }
  }
} 

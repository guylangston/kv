dotnet run -- init sample

dotnet run -- set sample hello world                              
dotnet run -- set sample guy langston                             
dotnet run -- set sample sentance "Lazy Brown Fox Jumped ..."     
dotnet run -- set sample withDoubleQuote "\"Hello\" world"
dotnet run -- set sample withSingleQuote "'hello' world"
dotnet run -- set sample empty ""
dotnet run -- set sample html "<tag id=\"123\">text</tag>"
dotnet run -- set sample url "https://www.skysports.com/nba?gr=www"

dotnet run -- ls sample                                           
dotnet run -- export sample                                       
dotnet run -- export sample -f html                                       

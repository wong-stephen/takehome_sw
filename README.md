# Take Home Project

Challenge: A directory contains multiple files and directories of non-uniform file and directory names. Create a program that traverses a base directory and creates an index file that can be used to quickly lookup files by name, size, and content type.

# Usage

I used C# to create a console application (.exe) for this project. I committed the source code and the binaries to run this project. 
To run the indexing, you can go to the net5.0 folder in either the debug or release folder and run this command -> [FileUtility -index (path of test_data)].
To run the search, you can go to that same folder I mentioned earlier and run this command -> [FileUtility -search (name|size|type) (search term) (path of test_data)].
I was able to run it on Windows using a command prompt so that I can see the output. 
Thanks again for the opportunity.
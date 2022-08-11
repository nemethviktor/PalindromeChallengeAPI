
## Task Description

> Write an application that finds the 3 longest unique
> palindromes in a supplied string. For the 3 longest ... palindromes,
> report the palindrome, start index and length, in descending order of
> length ... 
> Given the input string:
> sqrrqabccbatudefggfedvwhijkllkjihxymnnmzpop , the output should be:
> 
> Text: hijkllkjih, Index: 23, Length: 10 
> Text: defggfed, Index: 13, Length: 8 
> Text: abccba, Index: 5 Length: 6 

## Building & Running

- Code was built in VS Community 2022 and builds a windows-exe file that will run the API on localhost. 
- Once compiled either run in browser of cUrl as "curl -k https://localhost:5001/palindrome/aabbccdd" (port is random)
- See Routing for comment re: unicode chars in CMD-based executors.
- As per https://stackoverflow.com/questions/56511032/solution-has-projects-that-are-located-outside-the-solution-folder the MSTest.TestAdapter package doesn't get copied. This is a VS bug.

## Assumptions/known issues/etc.

Task requirement typo

- The last line of expected output as per taks requirement is missing a comma after "5". The script will assume this was a typo.

HTTP stuff:

 - Port is random and the app only listens on the assigned port (while I theoretically know how to change this, I didn't)
 - HTTP (as in not HTTPS) isn't handled
 - _theoretically_ non-compliant routes return a 418 error but in cUrl they never worked - I either got 405 or 200 so a bit unsure on
   this one

Routing:
- Route names are CS (case-sensitive)
- The script handles non-standard characters but this is also subject to running environment. (e.g anything run in Windows-based CMD such as cUrl will be problematic)
  - Eg "!"s are fine both in Chrome and cUrl but 
  - "#"s work like a string terminator in cUrl and anything after a "#" is ignored 
  - "#"s work ok in Chrome
  - 丽丽 will not get a response at all in cmd-executed cUrl
  - 丽丽 works ok in Chrome.
- Spaces are allowed but obviously 4 space characters after each other will count as a palindrome. 
- input has to be 2 to 160 chars. <2 chars can't really be palindromes and I think 160+ can overload the system/and-or is an overkill.

Palindromes:
- Since the definition of "unique" hasn't been specified I'm just taking the first one if there are more - as found by the code.
- Top 3 doesn't handle 3+ indentical-length items. The first three are shown, that's that.
- Script input is CS (case-sensitive) (e.g. "abBA" will return a fail)
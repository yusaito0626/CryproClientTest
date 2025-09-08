# Additional clean files
cmake_minimum_required(VERSION 3.16)

if("${CONFIG}" STREQUAL "" OR "${CONFIG}" STREQUAL "Release")
  file(REMOVE_RECURSE
  "CMakeFiles\\CryptoClient_GUI_autogen.dir\\AutogenUsed.txt"
  "CMakeFiles\\CryptoClient_GUI_autogen.dir\\ParseCache.txt"
  "CryptoClient_GUI_autogen"
  )
endif()

using System.Collections;

namespace ST
{
    public class MapUtils
    {
        /*
          function printArray(arr) {
              var str = "";
              if(!arr)
                  return;
              for (var i = 0; i < arr.length; i++) {
                  if(arr[i] && arr[i].length){
                      for (var j = 0; j < arr[i].length; j++) {
                          str += "," + arr[i][j];
                      }
                      str += "\n";
                  }
              }
              str += "\n";
              $('#output').append(str);
          }
          
          function ArrayTranspose(arr) {
              var arrNew = [];
              var n = arr.length;
              for (var i = 0; i < n; i++) {
                  var m = arr[i].length/2 + 1;
                  for (var j = 0; j < m; j++) {
                      if (!arrNew[i])
                          arrNew[i] = [];
                      arrNew[i][j] = arr[j][i];
                  }
              }
              return arrNew;
          }
          
          function ArrayRotateCCW(arr) {
              var arr = ArrayTranspose(arr);
              var arrNew = [];
              var n = arr.length;
              for (var i = 0; i < n; ++i) {
                  var m = arr[i].length/2 + 1;
                  for (var j = 0; j < m / 2; ++j) {
                      if (!arrNew[j])
                          arrNew[j] = [];
                      if (!arrNew[n - j - 1])
                          arrNew[n - j - 1] = [];
                      arrNew[j][i] = arr[n - j - 1][i];
                      arrNew[n - j - 1][i] = arr[j][i];
                  }
              }
              return arrNew;
          }
          
          function ArrayRotateCW(arr) {
              var arr = ArrayTranspose(arr);
              var arrNew = [];
              var n = arr.length;
              for (var i = 0; i < n; ++i) {
                  var m = arr[i].length/2 + 1;
                  for (var j = 0; j < m; ++j) {
                      if (!arrNew[i])
                          arrNew[i] = [];
                      //console.log(i,j);
                      arrNew[i][j] = arr[i][n - j - 1];
                      arrNew[i][n - j - 1] = arr[i][j];
                  }
              }
              return arrNew;
          }
          
          var orig = [
                      [1, 2, 3],
                      [4, 5, 6],
                      [7, 8, 9]
                      ];
          printArray(orig);
          
          $('#output').append("test1:\n");
          var test1 = ArrayTranspose(orig);
          printArray(test1);
          
          $('#output').append("test2:\n");
          var test2 = ArrayRotateCW(orig);
          printArray(test2);
          
          $('#output').append("test3:\n");
          var test3 = ArrayRotateCCW(orig);
          printArray(test3);
*/

        /*
        // Create enum without specifying actual values
        // when specific integers are unecessary
        // more complete version: https://github.com/rauschma/enums
        function Enum() {
            for (var i = 0; i < arguments.length; i++) {
                this[arguments[i]] = i;
            }
            
            if (Object.freeze)
                Object.freeze(this);
        }
        
        // Clear array in-place
        // array.length = 0 is slower than pop all elements
        // according to jsperf test: http://jsperf.com/array-destroy/32
        function ArrayClear(arr) {
            while (arr.length > 0) {
                arr.pop();
            }
        }
        
        function ArrayPrint(arr) {
            var n = arr.length;
            console.log("[");
            for (var j = 0; j < n; j++) {
                var m = arr[j].length;
                var rowArr = [];
                for (var i = 0; i < m; i++) {
                    var tile = arr[j][i];
                    var tileheight = tile ? (tile.height ? tile.height : 0) : 0;
                    rowArr.push(tileheight);
                }
                console.log("    " + rowArr.join(","));
            }
            console.log("]");
        }
        
        // WARNING: this assumes staggered iso 
        //  - with offset odd rows
        // trim excess rows/columns before any data or after all data
        function ArrayTrim(arr) {
            // trim pre-data rows/cols
            // - search for all columns of row 1 to N
            // - if no data, remove row
            // - if data, stop search
            var arrNew = [];
            var n = arr.length;
            var foundData = false;
            var rowIndex = 0;
            var nRemoved = 0;
            for (var j = 0; j < n; j++) {
                var m = arr[j].length;
                var rowNew = [];
                var isEmpty = true;
                for (var i = 0; i < m; i++) {
                    var tile = arr[j][i];
                    var tileID = tile ? (tile.tileID ? tile.tileID : 0) : 0;
                    var tileheight = tile ? (tile.height ? tile.height : 0) : 0;
                    // console.log('trim:', i, j, "=> tid:", tileID, "h:", tileheight);
                    if (tile) {
                        rowNew[i] = tile;
                        isEmpty = false;
                    }
                }
                // console.log('empty:', isEmpty);
                if (!isEmpty || foundData) {
                    foundData = true;
                    arrNew[rowIndex] = rowNew;
                    rowIndex++;
                } else {
                    nRemoved++;
                }
            }
            
            // Shift for change in odd rows
            if (nRemoved % 2 === 1) {
                console.log('need to shift for odd row offset');
                for (var j = 1; j < n; j += 2) {
                    var m = arr[j].length;
                    for (var i = 1; i < m; i++) {
                        if (arrNew[j]) {
                            arrNew[j][i - 1] = arrNew[j][i];
                        }
                    }
                }
            }
            
            arr = ArrayTranspose(arrNew);
            arrNew = [];
            
            n = arr.length;
            foundData = false;
            rowIndex = 0;
            nRemoved = 0;
            
            for (var j = 0; j < n; j++) {
                var m = arr[j].length;
                var rowNew = [];
                var isEmpty = true;
                for (var i = 0; i < m; i++) {
                    var tile = arr[j][i];
                    var tileID = tile ? (tile.tileID ? tile.tileID : 0) : 0;
                    var tileheight = tile ? (tile.height ? tile.height : 0) : 0;
                    // console.log('trim:', i, j, "=> tid:", tileID, "h:", tileheight);
                    if (tile) {
                        rowNew[i] = tile;
                        isEmpty = false;
                    }
                }
                // console.log('empty:', isEmpty);
                if (!isEmpty || foundData) {
                    foundData = true;
                    arrNew[rowIndex] = rowNew;
                    rowIndex++;
                } else {
                    nRemoved++;
                }
            }
            
            return ArrayTranspose(arrNew);
        }
        
        function ArrayTranspose(arr) {
            var arrNew = [];
            var n = arr.length;
            for (var j = 0; j < n; j++) {
                var m = arr[j].length;
                for (var i = 0; i < m; i++) {
                    var itrans = j;
                    var jtrans = i;
                    if (!arrNew[jtrans])
                        arrNew[jtrans] = [];
                    var tile = arr[j][i];
                    var tileID = tile ? (tile.tileID ? tile.tileID : 0) : 0;
                    var tileheight = tile ? (tile.height ? tile.height : 0) : 0;
                    // console.log('trans:', i, j, "=>", itrans, jtrans, tileID, "h:", tileheight);
                    arrNew[jtrans][itrans] = arr[j][i];
                }
            }
            return arrNew;
        }
        
        // trans, then reverse rows
        function ArrayRotateCCW(arr) {
            var arr = ArrayTranspose(arr);
            var arrNew = [];
            var n = arr.length;
            for (var j = 0; j < n; ++j) {
                var m = arr[j].length;
                for (var i = 0; i < m; ++i) {
                    var irot = i;
                    var jrot = n - j - 1;
                    if (!arrNew[jrot])
                        arrNew[jrot] = [];
                    arrNew[jrot][irot] = arr[j][i];
                }
            }
            return arrNew;
        }
        
        // trans, then reverse cols
        function ArrayRotateCW(arr) {
            var arr = ArrayTranspose(arr);
            var arrNew = [];
            var n = arr.length;
            console.log('cw: n = ', n);
            for (var j = 0; j < n; ++j) {
                var m = arr[j].length;
                // console.log('cw: m = ', m);
                for (var i = 0; i < m; ++i) {
                    if (!arrNew[j])
                        arrNew[j] = [];
                    var irot = m - i - 1;
                    var jrot = j;
                    var tile = arr[j][i];
                    var tileID = tile ? (tile.tileID ? tile.tileID : 0) : 0;
                    var tileheight = tile ? (tile.height ? tile.height : 0) : 0;
                    // console.log(i, j, "=>", irot, jrot, tileID, "h:", tileheight);
                    arrNew[jrot][irot] = arr[j][i];
                }
            }
            return arrNew;
        }
        
        function ArrayTransposeInplace(arr) {
            var arrLen = arr.length;
            for (var i = 0; i < arrLen; i++) {
                for (var j = 0; j < i; j++) {
                    //swap element[i,j] and element[j,i]
                    var temp = arr[i][j];
                    arr[i][j] = arr[j][i];
                    arr[j][i] = temp;
                }
            }
        }
        
        // transpose, then reverse rows
        function ArrayRotateCCWInplace(arr) {
            ArrayTranspose(arr);
            var n = arr.length;
            for (var i = 0; i < n; ++i) {
                for (var j = 0; j < n / 2; ++j) {
                    var tmp = arr[j][i];
                    arr[j][i] = arr[n - j - 1][i];
                    arr[n - j - 1][i] = tmp;
                }
            }
        }
        
        // transpose, then reverse columns
        function ArrayRotateCWInplace(arr) {
            ArrayTranspose(arr);
            var n = arr.length;
            for (var i = 0; i < n; ++i) {
                for (var j = 0; j < n / 2; ++j) {
                    var tmp = arr[i][j];
                    arr[i][j] = arr[i][n - j - 1];
                    arr[i][n - j - 1] = tmp;
                }
            }
        }
        
        
        // WARNING: 
        //   - current assumption is that was diamon-ified by
        //   - StaggeredToDiamond()
        //   - assume that iso diamond is square
        //   - assume staggerred map is also square
        // Convert from Square Tile Iso Diamond to Iso Staggered
        function DiamondToStaggered(arrIso) {
            // get dimensions
            var isoHeight = arrIso.length;
            var isoWidth = arrIso[0].length;
            var stagWidth = isoWidth;
            var stagHeight = isoWidth + isoHeight;
            
            console.log("iso dims: ", isoHeight, isoWidth);
            
            // create new array for "diamond"
            var arrStag = [];
            for (var j = 0; j < stagHeight; j++) {
                arrStag[j] = [];
                for (var i = 0; i < stagWidth; i++) {
                    arrStag[j][i] = null;
                }
            }
            
            console.log('Created Empty Array');
            console.log(arrStag);
            
            for (var j = 0; j < isoHeight; j++) {
                for (var i = 0; i < isoWidth; i++) {
                    var isoX = i;
                    var isoY = j;
                    // var isoX = Math.floor(stagY / 2) + stagX + (stagY & 0x01);
                    // var isoY = Math.floor(stagY / 2) - stagX + originY;
                    
                    // 0,0 => 1,0 (width 2 or 3)
                    // 0,0 => 2,0 (width 4 or 5)
                    // 0,0 => 3,0 (width 6 or 7)
                    var stagY = isoX + isoY;
                    var offX = stagY & 0x01 ? -1 : 0;
                    var stagX = Math.floor(isoWidth / 2) + Math.ceil((isoX + offX) / 2.0) - Math.ceil(isoY / 2.0);
                    
                    isoX = isoX >> 0;
                    isoY = isoY >> 0;
                    stagX = stagX >> 0;
                    stagY = stagY >> 0;
                    
                    if (!arrStag[stagY]) {
                        arrStag[stagX] = [];
                    }
                    
                    if (arrIso[isoY]) {
                        var tile = arrIso[isoY][isoX];
                        var tileID = tile ? (tile.tileID ? tile.tileID : 0) : 0;
                        var tileheight = tile ? (tile.height ? tile.height : 0) : 0;
                        // console.log("iso->stag:", isoX, isoY, "=>", stagX, stagY, tileID, "h:", tileheight);
                        arrStag[stagY][stagX] = arrIso[isoY][isoX];
                    }
                }
            }
            return arrStag;
        }
        
        // Convert from Square Tile Iso Staggered to Iso Diamond 
        // ISO: isometric diamond 
        // STAG: isometric staggered 
        function StaggeredToDiamond(arrStag) {
            // get dimensions
            var stagHeight = arrStag.length;
            var stagWidth = arrStag[0].length;
            var isoWidth = stagWidth + Math.floor(stagHeight / 2);
            //var isoHeight = stagWidth + Math.floor(stagHeight / 2);
            var isoHeight = isoWidth;
            
            console.log("stag dims: ", stagHeight, stagWidth);
            
            // create new array for "diamond"
            var arrIso = [];
            console.log(arrIso);
            for (var j = 0; j < isoHeight; j++) {
                arrIso[j] = [];
                for (var i = 0; i < isoWidth; i++) {
                    arrIso[j][i] = null;
                }
            }
            
            console.log('Created Empty Array');
            console.log(arrIso);
            
            var n = stagHeight;
            var m = stagWidth;
            var originY = n - 1;
            for (var j = 0; j < stagHeight; j++) {
                var rowWidth = stagWidth;
                for (var i = 0; i < rowWidth; i++) {
                    var stagX = i;
                    var stagY = j;
                    var isoX = Math.floor(stagY / 2) + stagX + (stagY & 0x01);
                    var isoY = Math.floor(stagY / 2) - stagX + originY;
                    if (!arrIso[isoY]) {
                        arrIso[isoY] = [];
                    }
                    var tile = arrStag[stagY][stagX];
                    var tileID = tile ? (tile.tileID ? tile.tileID : 0) : 0;
                    var tileheight = tile ? (tile.height ? tile.height : 0) : 0;
                    // console.log("stag->iso:", stagX, stagY, "=>", isoX, isoY, tileID, "h:", tileheight);
                    arrIso[isoY][isoX] = tile;
                }
            }
            return arrIso;
        }
        
        var seed = getParameterByName("rngseed");
        console.log("seed = " + seed);
        if(seed !== "") {
            seed = Math.seedrandom(seed);
        } else {
            Math.seedrandom(); 
            seed = Math.random().toString(36).slice(2,20);
            //seed = Math.random().toString(10).slice(2);
            Math.seedrandom(seed); 
        }
        console.log("rng seed: " + seed);
        
        
        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
                                  var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                                  results = regex.exec(location.search);
                                  return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
                                  }
                                  
                                  // Returns a random number between 0 (inclusive) and 1 (exclusive)
                                  function getRandom() {
                return Math.random();
                                  }
                                  // Returns a random number between min and max
                                  function getRandomArbitrary(min, max) {
                return Math.random() * (max - min) + min;
                                  }
                                  // Returns a random integer between min and max
                                  // Using Math.round() will give you a non-uniform distribution!
                                  function getRandomInt(min, max) {
                return Math.floor(Math.random() * (max - min + 1) + min);
                                  }
*/

    }
}

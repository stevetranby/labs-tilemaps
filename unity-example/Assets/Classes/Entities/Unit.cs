using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ST
{
    /// <summary>
    /// Unit. or Worker
    /// 
    /// A specific type of entity that does work for a player or a main entity or small set of hero entities.
    /// 
    /// </summary>
    public class Unit // : Entity
    {
//  var Unit = cc.Class.extend({
//      
//  waypoints:[],
//  nextTask:null,
//  nextWaypoint:null,
//      
//  ctor: function() {
//          this._super();
//      },
//  update: function(dt) {
//          // move toward next waypoint
//          if(this.nextWaypoint) {
//              // test how far from
//              var pos = this.getPosition();
//              var diff = cc.pSub(pos,this.nextWaypoint);
//              var velocity = cc.pNormalize(diff);
//              console.log("velocity", velocity);
//              
//              // TODO: should be based on speed
//              var speed = 5.0;
//              var threshold = 2.0;
//              
//              var newPos = cc.pAdd(pos, cc.pMult(velocity, speed));
//              
//              if(Math.abs(dx) > threshold || Math.abs(dx) > threshold) {
//                  // make sure to damp 
//                  // maker sure to not oscillate
//              } else {
//                  newPos = this.nextWaypoint;
//              }
//              
//              this.setPosition(newPos);
//          } 
//      },
//      
//  isAvailableForWork: function() {
//          return (this.nextWaypoint === null);
//      },
//      
//  workRequested: function(waypoint, task) {
//          if(this.isAvailableForWork()) {
//              this.nextTask = task;
//              this.nextWaypoint = waypoint;
//              return true;
//          }
//          return false;
//      }
//  });



//    updateUnit: function() {
//            
//            var tw = this.map.tileWidth;
//            var th = this.map.tileWidth / 2.0;
//            
//            var newTile = this.player.currentTile;
//            var r = newTile.y;
//            if (this.map && this.map.tiles && this.map.tiles[r]) {
//                var c = newTile.x;
//                
//                // trees and stuff
//                var tileInfo = this.map.tiles[r] ? this.map.tiles[r][c] : null;
//                if (tileInfo) {
//                    
//                    var zOrder = r + c >> 0;
//                    var x = tw * this.map.cols / 2 + ((c - r) * tw / 2);
//                    var y = th * this.map.rows - ((c + r) * th / 2);
//                    var z = tileInfo.height;
//                    
//                    x = x >> 0;
//                    y = y >> 0;
//                    z = z >> 0;
//                    
//                    var yOffFromZ = y + z * th;
//                    this.player.setPosition(cc.p(x, yOffFromZ + th / 2.0));
//                    this.player.setZOrder(zOrder);
//                }
//            }
//        },
//
    }
}
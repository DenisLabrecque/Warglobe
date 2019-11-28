# Warglobe
Unity airplane and ship simulation on a planetary sphere.

My vision for this project is slow growth over a decade or so until a complete game is made for my accomplishment and amusement.

## Features Implemented
- Flight
- HUD
- Radar and line-of-sight system
- Automatic level flight

[![](http://img.youtube.com/vi/7oSqnHO_2E0/0.jpg)](http://www.youtube.com/watch?v=7oSqnHO_2E0 "Game Example")

## Features Being Worked On
- Flotation
- Waypoints
- User interface
- Gameplay

[![](http://img.youtube.com/vi/yPUPlVAfM2c/0.jpg)](http://www.youtube.com/watch?v=yPUPlVAfM2c "Flotation")

## Controls
<table>
<thead>
<tr>
<th>Function</th>
<th>Control</th>
</tr>
</thead>
<tbody>
<tr>
<td>Bank left</td>
<td>left arrow (and joystick)</td>
</tr>
<tr>
<td>Bank right</td>
<td>right arrow (and joystick)</td>
</tr>
<tr>
<td>Pull up</td>
<td>down arrow (and joystick)</td>
</tr>
<tr>
<td>Pull down</td>
<td>up arrow (and joystick)</td>
</tr>
<tr>
<td>Yaw left</td>
<td>q (and joystick twist)</td>
</tr>
<tr>
<td>Yaw right</td>
<td>e (and joystick twist)</td>
</tr>
<tr>
<td>Accelerate</td>
<td>left shift (and throttle)</td>
</tr>
<tr>
<td>Decelerate</td>
<td>z (and throttle)</td>
</tr>
<tr>
<td>Cycle cameras</td>
<td>c</td>
</tr>
<tr>
<td>Planet view</td>
<td>m</td>
</tr>
<tr>
<td>Shoot laser</td>
<td>left ctrl</td>
</tr>
</tbody>
</table>


### Issues
This game is a work in progress. Certain elements noted below do not work yet; if it's on the list, I'm eventually fixing it.

#### Trivial Problems
- Translation strings do not work in game mode
- Some airplane parts don't collide with the ground
- Camera can pass through objects
- Aircraft wheel bays don't open
- Aircraft wheels have no suspension
- Airplane wheels are not correctly set in/out at game start
- Airplane throttle setting is not correct at game start

#### Functional Issues
- The planet view behaves strangely and cannot zoom
- The airplane will jitter and move continuously when landed
- The HUD view is still present in planet view
- Aircraft cannot land on aircraft carrier for lack of a tail hook implementation
- The FPV camera view is static when it should swivel inside the cockpit
- There is no flight path marker (FPM)
- Batteries cannot be recharged
	
#### Missing Features
- Multiplayer playability (waiting for Unity connected games)
- Decals and flags with country logos
- The player can change between vehicles
- Airplanes can mount bombs and missiles
- Pause screen and credits screen
- The player can earn new vehicles
- Include waypoint and city tags
- Aircraft carriers have elevators and hangars
- Give the game a goal (or levels)

## Intended Aesthetic
<img src="https://i.pinimg.com/originals/4c/1b/1f/4c1b1fe91776ce3941705da87e1f6e31.jpg" />
<img src="https://i.pinimg.com/originals/0f/5e/8c/0f5e8c23e2beb6c799514a6b94ca61f0.jpg" />
<img src="https://i.pinimg.com/originals/8c/cf/05/8ccf05f0d64588b6bd6ae491004610b2.jpg" /><img src="https://i.pinimg.com/originals/42/a4/a2/42a4a27c79ddc5da007de54ae21f07db.jpg" />
These images are for inspiration only.

## Factions

### Allied Coalition
#### Gomer
<img src="Assets/Resources/Flags/RoundelGomer_2.png" />
Demonym: Gomerag
Capital: Galic

#### Scythia
<img src="Assets/Resources/Flags/RoundelScythia.png" />
Demonym: Scyth
Capital: Ukra

### United Coalition
#### Javan
<img src="Assets/Resources/Flags/RoundelJavan.png" />
Demonym: Javanite
Capital: Elys

#### Tobolik
<img src="Assets/Resources/Flags/RoundelTobolik.png" />
Demonym: Tabali
Capital: Tobolsk

### International Coalition
#### Meshchera
Demonym: Meschek
Capital: Meshkva

#### Thuras
Demonym: Thuran
Capital: Thur

#### Cush
<img src="Assets/Resources/Flags/RoundelCush.png" />
Demonym: Cushite
Capital: Ethi

### Cosmopolitan Coalition
#### Mizr
Demonym: Mizraim
Capital: Mitz

#### Huttia
Demonym: Huttite
Capital: Hut

### Federal Coalition
#### Elam
Demonym: Elami
Capital: Eru

#### Asshuria
<img src="Assets/Resources/Flags/RoundelAsshuria.png" />
Demonym: Asshur
Capital: Ashkh

#### Arphaxia
Demonym: Arrhip
Capital: Chalda

### Joint Coalition
#### Lydda
Demonym: Sardi
Capital: Lydd

#### Aram
Demonym: Arami
Capital: Aramecc

### No Coalition/Rogue
#### Madesh
<img src="Assets/Resources/Flags/RoundelMadesh.png" />
Demonym: Madai
Capital: Madapa

#### Palesti
<img src="Assets/Resources/Flags/RoundelPalesti.png" />
Demonym: Philisim
Capital: Jebussa

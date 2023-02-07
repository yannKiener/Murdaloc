#  Documentation

### Introduction

Seul le contenu dossier "Assets" est documenté.
OutLineEffect étant une dépendance nous n'en parlerons pas.

### 1. Scenes 

TODO LATER

### 2. Resources
Contient pas mal de choses utilisé dans le code du jeu.
#### 2.1. Images
 * Background : contient tout ce qui sert au "Parallax". C'est les 2eme et 3eme plans.
 * GUI : contient tout ce qui est lié à l'interface graphique.
 * Items : contient toutes les images des items
 * Maps : Sert pour la map principale a laquelle le reste du monde créé est "rattaché".
 * RandomUsableThings : Contient toutes les images marquées comme potentiellement utilisables
 * SpellIcons : contient toutes les images des sorts.
 * Textures : Guess what !? Les textures. Pour l'instant y'en a qu'une de toutes facon.
 
Les autres fichiers qui trainent hors de ces dossiers sont uniquement pour tester.

#### 2.2. Musics
Contient toutes les musiques utilisables en jeu. (Il n'y aura toujours qu'une musique en lecture a la fois)

#### 2.3. Sounds
Contient tous les sons utilisables en jeu. (plusieurs sons peuvent être lus en même temps contrairement aux musiques.)


### 3. Scripts
Contient tout le code du jeu ! 
#### 3.1. Characters
On y retrouve les classes définissant les personnages.
**Character** est la classe mère (virtuelle) et elle a 2 sous-classes.
 * **Hostile** : Définit le comportement d'un personnage hostile envers le joueur.
 * **Player** : Définit le comportement du personnage joueur.
Eh ouais y'a pas de Friendly pour l'instant. 
Chaque **Character** possède des **Stats** et une **Resource**.
Les **Stats** correspondes aux classiques {Force, endurance, agilité, critique, ...}
La **Resources** est une interface avec 3 implémentations (Dans la même classe... oui.) : Mana, Energy, Rage.

#### 3.2. Actions
Actions contient les classes correspondant aux actions que peuvent faire les characters impactant l'environnement
On y retrouve notemment la classe **Spell**. Cette classe, comme pour **Character** est une classe virtuelle avec 2 sous classes : 
 * **HostileSpell** : correspond aux sorts a jeter sur un enemi
 * **FriendlySpell** : correspond aux sorts a jeter sur un allié ou sur soi.
Chaque **Spell** contient potentiellement un effet avec une durée précise. 
La classe **EffectOnTime** est faite dans ce but. Un **EffectOnTime** peut également avoir un **Effect** a appliquer pendant sa durée (Notamment *StatEffect* pour modifier des stats en pourcentage pendant la durée de l'**EffectOnTime**.

#### 3.3. Initialisation
Ce dossier contient tout ce qui sert a l'initialisation. Les classes ici sont donc expressément instanciées en premier.
Contient uniquement la classe **SpellAndEffectLoader**.
C'est ici qu'on charge tous les sorts en mémoire, donc pour en créer, c'est ici !

#### 3.4. Interface
On y retrouve tout ce qui définit le comportement de l'interface utilisateur : 
Il y a une partie d'affichage uniquement : 
 * **StatusBar** : Correspond a la barre d'état qui s'affiche au dessus des Characters en combat
 * **Interface** : On y retrouve les fonctions pour la barre de cast, les tooltip, plus tard les bulles de texte, les infos sur un Character.
 * **FloatingText** : Sert a définir le comportent des textes flottants : Dégats, messages d'erreur, ...
Et une partie avec laquelle le joueur interagit  (Ou on peut notamment est Drag / Drop)
 * **ActionBar** :  C'est la barre d'action avec raccourcis dans laquelle le joueur peut placer tout ce qui est utilisable.
 * **CharacterSheet** : C'est la fiche de personnage avec notamment l'équipement
 * **SpellBook** : C'est le contenant des sorts du joueur.
Tout ce qui est Drag & Droppable fonctionne de la manière suivante : 
Il y a un GameObject parent avec un des script présenté plus haut. ils implémentent tous l'interface **Slotable** qui définit leur comportement en cas de Drag ou de Drop.
Le parent contiendra des GameObject ayant le script **Slot**. Ces Slot sont des containers qui :
 * Appellement les implémentations de Slotable
 * contiennent un élément implémentant l'interface **Usable** (Exemple : un sort, un item, ...)
 * sont cliquables. Dans ce cas ils appellent la méthode Use().
 
#### 3.5. Items 
Contient tout ce qui concerne les Items.
On y retrouve **Items** : qui implémente l'interface Usable. Contient également des **Stats**.

#### 3.6. Map & Camera
Contient : 
 * Le générateur de map **MapGenerator** : Ce script est a attacher a un GameObject. Il s'occupe de générer la map avec les mobs et les arbres avec densité aléatoirement ou non. Voir directement dans la Scène dans Unity ;)
 * **CameraFollowPlayer** : Définit le comportement de la caméra qui suit le joueur, sauf en cas de combat.
 * **ScrollingBackground** : Définit le comportement des arrières plans.

#### 3.7. Mobs 
Contient tous les différents mobs possibles (Leur nom, leur sorts disponnible, ...). Pour les utiliser, il faut d'abord créer un Prefab avec le script et le passer en paramètre du **MapGenerator** (voir plus haut).

#### 3.8. Player 
Dossier potentiellement éphémère. On y retrouve :
 * **PlayerController** : Permet la création du joueur
 * **TrollGameOverScreen** : Qui affiche quelque chose quand le joueur crève.

#### 3.9 Sound
On y retrouve tout ce qui est lié a la musique et au son : 
**MusicManager** et **SoundManager** (d'ici peu! Eh oui on travaille en DF : Documentation First ;) ;) ;) )

#### 3.0. Utils
On y retrouve des classes qui sont appellées par plusieurs autres.
 * **FindUtils** : Sert a retrouver les objets les plus courants
 * **InterfaceUtils** : Tout ce qui sert a importer des images, ou encore a créer des **Slots**. 
 * **MessageUtils** : Tout ce qui sert a afficher des messages pour l'utilisateur.


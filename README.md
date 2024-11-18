# Unity小游戏——Hit UFO

[toc]

## 游戏背景与设定

> 游戏设定在一个充满未知与挑战的外太空环境中，玩家扮演一名太空守护者，任务是击落不断来袭的飞碟（UFO）。这些飞碟来自不同的外星文明，它们试图侵扰地球的安全，玩家需要迅速反应并准确击中这些飞碟，以保护地球免受威胁
>
> 1. **游戏目标**：玩家的主要目标是在有限的时间内，尽可能多地击中飞碟，以获得高分。每击中一个飞碟，玩家都将获得相应的分数，分数的高低取决于飞碟的颜色、大小、速度等因素
>
> 2. **游戏模式**：
>    - **正常模式**：游戏通常分为多个回合（round），每个回合包含一定数量的试验（trial）。例如，有的版本设定有5个回合，每个回合包括10次试验。随着回合数的增加，游戏的难度也会相应提升，飞碟的属性（如速度、数量等）会变得更加复杂和难以应对
>    - **无尽模式**：在某些版本中，游戏还提供了无尽模式，玩家可以无限制地继续游戏，挑战自己的极限
>
> 3. **飞碟属性**：每个飞碟都具有独特的属性，包括颜色、大小、发射位置、速度、角度等。这些属性在每次试验中都是随机生成的，为玩家提供了不同的挑战和变化。飞碟的颜色通常与分数相关联，例如红色飞碟得1分，绿色得2分，蓝色得3分。飞碟的大小和速度也会影响其得分和击中的难度
>
> 4. **游戏控制**：玩家通过鼠标点击来击中飞碟。在游戏界面上，飞碟会从不同的位置和角度出现，玩家需要迅速反应并准确点击以击中它们
>
> 5. **游戏结束条件**：正常模式下，当所有回合的飞碟都被击落或达到预定的时间限制时，游戏将结束。此时，玩家可以查看自己的最终得分，并与其他玩家进行比较
>
> 6. **游戏优化**：为了提高游戏的性能和可玩性，开发者通常会采用对象池、工厂模式等设计模式来管理飞碟的创建和回收。这些技术可以减少内存分配和垃圾回收的开销，提高游戏的流畅度和响应速度

<div style="text-align: right;">——以上内容为AI 辅助创作</div>



## 效果展示

![QQ20241118-132606](images\QQ20241118-132606.png)

![QQ20241118-132656](images\QQ20241118-132656.png)

> 项目地址：https://github.com/NAGenius/HitUFO



## 设计思路

### 资源获取

> 资源获取始终是开发游戏的门槛之一，一个好的程序员总不见得是一个好的艺术家，这时候又该提到......🤔
>
> 本项目的资源均来自：[Unity 资源商城](https://assetstore.unity.com)

- **UFO**：https://assetstore.unity.com/packages/3d/vehicles/space/ufo-battleship-289193
- **Skybox**：https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633

![QQ20241118-121721](images\QQ20241118-121721.png)![QQ20241118-121600](images\QQ20241118-121600.png)



### 资源使用

> 将获取的资源添加到我的资源后，可以在 Unity 项目中通过 `Window -> Package Manager` **选择性进行 import**

![QQ20241118-121919](images\QQ20241118-121919.png)

- **UFO**：加载 `.prefab` 文件即可

```c#
Resources.Load("Prefabs/UFO", typeof(GameObject))
```

- **Skybox**：将 `.mat` 文件挂载到 `Window -> Rendering -> Lighting` 上即可

![QQ20241118-122530](images\QQ20241118-122530.png)



### UML图

> 这里给出 Controllers 部分的 UML 图，Actions、Views 部分类似

![QQ20241118-124813](images\QQ20241118-124813.png)



### 目录结构

> 使用 `tree` 命令得到 `Assets` 的目录结构如下：

```sh
tree .

├─Resources
│  ├─Material
│  └─Prefabs
├─Scenes
├─Scripts
│  ├─Actions
│  ├─Controllers
│  └─Views
└─SkySeries Freebie
    └─FreebieHdri
```



### 核心设计与实现

#### 飞碟运动

> 飞碟运动其实是一个比较复杂的物理运动过程，这里对其进行简化操作：
>
> - 使用 Unity 提供的 `Rigidbody` 刚体部件进行模拟
> - 简单采用抛物线公式对速度进行模拟

```c#
Vector3 position;
position.x = origin_position.x + speedX * time;
position.y = origin_position.y + speedY * time - 0.5f * 5f * time * time;
position.z = origin_position.z;
```

#### 飞碟仓库

> 缓冲池是一种内存中的数据结构，用于缓存从较慢的存储设备（如硬盘）读取的数据，以减少对慢速设备的访问次数，从而提高数据访问速度。其主要目的是通过减少对慢速设备的访问，提高系统性能
>
> 因此，我们可以实现一个飞碟仓库，用缓冲池技术管理飞碟对象（加载、释放、回收飞碟），避免频繁创建与销毁对象的性能开销，以下为释放飞碟函数代码：

```c#
disk.SetActive(false);
disk.transform.position = new Vector3(0, 0, 0);
disk.transform.localScale = new Vector3(2, 0.1f, 2);
if (!used_UFO.Contains(disk))
{
    throw new MyException("Error: The disk to be freed is not in used_UFO list!");
}
used_UFO.Remove(disk);
free_UFO.Add(disk);
```

#### 飞碟降落判断

> 一个显而易见的问题就是：当飞碟降落在地面上时，应该如何处理？🤔
>
> 直接销毁？似乎是一个不错的方案，但其实考虑到上述飞碟仓库的缓冲池技术，我们可以将已经降落在地面上的飞碟就行回收，以达到节约资源的目的



#### 记分员类

> 和上次的裁判类其实比较类似，用于独立管理得分逻辑，本质上还是一个模块划分的思想，以便未来的维护和扩展🤔

```c#
using UnityEngine;

public class ScoreRecorder : MonoBehaviour
{
    int score;
    public RoundController round_ctrl;
    public UserGUI userGUI;
    // Start is called before the first frame update
    void Start()
    {
        round_ctrl = (RoundController)SSDirector.getInstance().currentSceneController;
        round_ctrl.score_recorder = this;
        userGUI = gameObject.GetComponent<UserGUI>();
    }

    public void Record(GameObject disk)
    {
        int score_onetime = disk.GetComponent<Disk_Attributes>().score;
        userGUI.gameMessage = "+" + score_onetime;
        score = score + score_onetime;
        userGUI.score = score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
```



## 结语

> 🎉🎉🎉，虽然算是做完了，但是整体效果还是不太好，因为做的时间实在是比较仓促，留待以后补充吧~~

# rl-JumpShoot

### Tech Stack
- [Unity2D(C#)](https://unity3d.com/machine-learning)
- [ML-Agents](https://github.com/Unity-Technologies/ml-agents)

### Setup
Random generate ground that allow agent to jump toward North direction

![GamePlay](https://raw.githubusercontent.com/weiweitoo/rl-JumpShoot/master/gameplay.JPG?token=AX6JDS6NgXXNt-wA9qmQhWUIWn7H289Mks5cvBa_wA%3D%3D "GamePlay")

 
 
### Goal
Jump to the higher ground
 
### Observation Space
- Raycast to North Direction with degree(30,45,60,90,120,135,150). *(0deg start from west)*
- Agent position x in world space
- Moving position of standing ground
 
### Action space
 - Jump(0,1)
 - Shoot(0,1)
 
### Reward Function
![MDP](https://raw.githubusercontent.com/weiweitoo/rl-JumpShoot/master/MDP.JPG?token=AX6JDTqAALTI0pDSqFNf9R-RLRQH6SXbks5cvBbQwA%3D%3D "MDP")
 
### Training
- Number of Step = 60000
- Algorithm = Proximal Policy Optimization(PPO)

#### Lesson
4 Lesson in total

Lesson 1
 - Ground width = 5
 - Number of ground = 3

Lesson 2
 - Reward threshold = 1.5f
 - Ground width = 5
 - Number of ground = 6
 
Lesson 3
 - Reward threshold = 4.0f
 - Ground width = 5
 - Number of ground = 8

Lesson 4
 - Reward threshold = 6.0f
 - Ground width = 5
 - Number of ground = 10
 
 
Trainer Configuration
```
    trainer: ppo
    batch_size: 1024
    beta: 5.0e-3
    buffer_size: 10240
    epsilon: 0.2
    gamma: 0.995
    hidden_units: 128
    lambd: 0.9
    learning_rate: 3.0e-4
    max_steps: 5.0e4
    memory_size: 256
    normalize: false
    num_epoch: 3
    num_layers: 3
    time_horizon: 64
    sequence_length: 64
    summary_freq: 5000
    use_recurrent: false
    use_curiosity: false
    curiosity_strength: 0.04
    curiosity_enc_size: 128
```

Agent Configuration
```
{
    "measure" : "reward",
    "thresholds" : [1.5, 4.0, 6.0],
    "min_lesson_length" : 10,
    "signal_smoothing" : true, 
    "parameters" : 
    {
        "ground_width" : [5, 5, 5, 5],
        "initial_ground" : [3, 6, 8, 10]
    }
}
```

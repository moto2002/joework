using UnityEngine;
using System.Collections.Generic;

public class TestMsg
{

}

/// <summary>
/// 帧同步
/// 收到的消息包都缓存在一个队列里面
/// 每隔一个逻辑帧，把这个队列里面的所有消息都拿出执行
/// </summary>
public class LogicFrameSync
{
    /// <summary>
    /// 消息队列
    /// </summary>
    Queue<TestMsg> messages = new Queue<TestMsg>();

    /// <summary>
    /// 存储每一帧要做的所有消息操作
    /// </summary>
    Dictionary<int, List<TestMsg>> frameLogics = new Dictionary<int, List<TestMsg>>();
}

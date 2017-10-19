using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using XZ.ParseLanguage.Entity;

namespace XZ.ParseLanguage.Parser {
    /// <summary>
    /// 行解析
    /// </summary>
    public class LineAnnotation {

        private Queue<LineAnnInfo> LineDatas = new Queue<LineAnnInfo>();

        private Operate pOperate;

        #region 创建实例

        private static object lockObjcet = new object();
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="operate"></param>
        private LineAnnotation(Operate operate) {
            this.pOperate = operate;
            ThreadPool.QueueUserWorkItem(new WaitCallback(Work));
        }

        private static LineAnnotation instance;

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="operate"></param>
        /// <returns></returns>
        public static LineAnnotation Instance(Operate operate) {
            if (instance == null) {
                lock (lockObjcet) {
                    if (instance == null)
                        instance = new LineAnnotation(operate);
                }
            }
            return instance;
        }

        #endregion

        /// <summary>
        /// 添加到线程中
        /// </summary>
        /// <param name="info"></param>
        public void Add(LineAnnInfo info) {
            LineDatas.Enqueue(info);
        }

        /// <summary>
        /// 无限循环
        /// </summary>
        /// <param name="arg"></param>
        private void Work(object arg) {
            while (true) {
                if (LineDatas.Count > 0) {
                    var info = this.LineDatas.Dequeue();
                    this.pOperate.ReadLineAnnotation(info);
                }
                Thread.Sleep(1000);
            }
        }
    }

}

using AuditManager.Common;
using AuditManager.Model;
using IManage;
using System;
using System.Collections.Generic;

namespace IM.Mgr
{
    public class WsUsrMgmt
    {
        public static void RemoveUsrFromGrp(string wsId, string grpName, string usrIdToRemove, ImDbType imDbType = ImDbType.Active)
        {
            //IMANADMIN.INRTUser inrtUsr = NrtSession.GetInstance.GetDb(imDbType).GetUser(AmUtil.GetCurrentUser);

            //--V469-583-S
            //IMANADMIN.INRTGroup inrtGrp = NrtSession.GetInstance.GetDb(imDbType).GetGroup(grpName);
            IMANADMIN.INRTGroup inrtGrp = NrtSession2.AdminDb(imDbType).GetGroup(grpName);
            //--V469-583-E

            inrtGrp.DeleteUser(usrIdToRemove);

            //IManWorkspace imWs = WsUtility.GetObjectByID<IManWorkspace>(wsId);
            //imWs.Security.GroupACLs.ItemByName("").Group.Users.re
        }

        public static void AddUsrToGrp(string wsId, string grpName, string usrIdToAdd, ImDbType imDbType = ImDbType.Active)
        {
            //--V469-583-S
            //IMANADMIN.INRTGroup inrtGrp = NrtSession.GetInstance.GetDb(imDbType).GetGroup(grpName);
            IMANADMIN.INRTGroup inrtGrp = NrtSession2.AdminDb(imDbType).GetGroup(grpName);
            //--V469-583-E

            inrtGrp.AddUser(usrIdToAdd);

        }

        public static List<WsUser> SearchUsr(string searchStr, UsrSearchBy usrSearchBy, bool isExactSrch = false, ImDbType imDbType = ImDbType.Active)
        {
            //--V469-583-S
            //IManDatabase imDb = ImSession.GetInstance.GetDb(imDbType);
            IManDatabase imDb = IManageSession.Db(imDbType);
            //--V469-583-E

            IManUsers imUsrs = null;
            bool srchIncludeDomain = false;

            List<WsUser> wsUsrs = new List<WsUser>();

            if (usrSearchBy == UsrSearchBy.Name)
            {
                //if (isExactSrch)
                //{
                //    imUsrs = imDb.SearchUsers(searchStr, imSearchAttributeType.imSearchExactMatch, true);
                //}
                //else
                //{
                //    imUsrs = imDb.SearchUsers(searchStr, imSearchAttributeType.imSearchFullName, true);
                //}

                imUsrs = imDb.SearchUsers(searchStr, imSearchAttributeType.imSearchFullName, true);
            }
            else if (usrSearchBy == UsrSearchBy.Email)
            {
                var spEmail = searchStr.Split('@');
                if (spEmail.Length == 1)
                {
                    //searchStr = email;
                }
                else if (spEmail.Length == 2)
                {
                    if (string.IsNullOrWhiteSpace(spEmail[1])
                        || spEmail[1].StartsWith("kpmg", StringComparison.OrdinalIgnoreCase)
                        || spEmail[1].StartsWith("kpmg.com", StringComparison.OrdinalIgnoreCase))
                    {
                        srchIncludeDomain = true;
                        searchStr = spEmail[0];
                    }
                    else
                    {
                        //searchStr = email;
                    }
                }
                else
                {
                    //searchStr = email;
                }

                //if (isExactSrch)
                //{
                //    imUsrs = imDb.SearchUsers(searchStr, imSearchAttributeType.imSearchExactMatch, true);
                //}
                //else
                //{
                //    imUsrs = imDb.SearchUsers(searchStr, imSearchAttributeType.imSearchID, true);
                //}

                imUsrs = imDb.SearchUsers(searchStr, imSearchAttributeType.imSearchID, true);
                
            }

            if (imUsrs !=  null)
            {
                if (usrSearchBy == UsrSearchBy.Name)
                {
                    if (isExactSrch)
                    {
                        foreach (IManUser imUsr in imUsrs)
                        {
                            var spFullName = searchStr.Split(',');

                            if (spFullName[0].Equals(imUsr.FullName.SplitNGet(',', 0), StringComparison.OrdinalIgnoreCase))
                            {
                                wsUsrs.Add(IM.Mgr.WsUtility.GetWsUser(imUsr));
                            }
                        }
                    }
                    else
                    {
                        foreach (IManUser imUsr in imUsrs)
                        {
                            wsUsrs.Add(IM.Mgr.WsUtility.GetWsUser(imUsr));
                        }
                    }
                }
                else if (usrSearchBy == UsrSearchBy.Email)
                {
                    if (isExactSrch)
                    {
                        foreach (IManUser imUsr in imUsrs)
                        {
                            if (imUsr.Name.Equals(searchStr, StringComparison.OrdinalIgnoreCase))
                            {
                                wsUsrs.Add(IM.Mgr.WsUtility.GetWsUser(imUsr));
                            }  
                        }
                    }
                    else
                    {
                        if (srchIncludeDomain)
                        {
                            foreach (IManUser imUsr in imUsrs)
                            {
                                if (imUsr.Name.Equals(searchStr, StringComparison.OrdinalIgnoreCase))
                                {
                                    wsUsrs.Add(IM.Mgr.WsUtility.GetWsUser(imUsr));
                                }
                            }
                        }
                        else
                        {
                            foreach (IManUser imUsr in imUsrs)
                            {
                                wsUsrs.Add(IM.Mgr.WsUtility.GetWsUser(imUsr));
                            }
                        }
                    }
                }

                //foreach (IManUser imUsr in imUsrs)
                //{
                //    if(srchIncludeDomain)
                //    {
                //        if(imUsr.Name.Equals(searchStr, StringComparison.OrdinalIgnoreCase))
                //        {
                //            wsUsrs.Add(IM.Mgr.WsUtility.GetWsUser(imUsr));
                //        }     
                //    }
                //    else
                //    {
                //        wsUsrs.Add(IM.Mgr.WsUtility.GetWsUser(imUsr));
                //    }
                //}
            }

            return wsUsrs;
        }
    }
}
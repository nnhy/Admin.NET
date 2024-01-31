/* tslint:disable */
/* eslint-disable */
/**
 * Admin.NET 通用权限开发平台
 * 让 .NET 开发更简单、更通用、更流行。前后端分离架构(.NET6/Vue3)，开箱即用紧随前沿技术。<br/><a href='https://gitee.com/zuohuaijun/Admin.NET/'>https://gitee.com/zuohuaijun/Admin.NET</a>
 *
 * OpenAPI spec version: 1.0.0
 * Contact: 515096995@qq.com
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */

import { AccountTypeEnum } from './account-type-enum';
import { CardTypeEnum } from './card-type-enum';
import { CultureLevelEnum } from './culture-level-enum';
import { GenderEnum } from './gender-enum';
import { StatusEnum } from './status-enum';
import {
    AccountTypeEnum,CardTypeEnum,CultureLevelEnum,GenderEnum,StatusEnum,
} from ".";

/**
 * 系统用户表
 *
 * @export
 * @interface SysUser
 */
export interface SysUser {

    /**
     * 雪花Id
     *
     * @type {number}
     * @memberof SysUser
     */
    id?: number;

    /**
     * 创建时间
     *
     * @type {Date}
     * @memberof SysUser
     */
    createTime?: Date | null;

    /**
     * 更新时间
     *
     * @type {Date}
     * @memberof SysUser
     */
    updateTime?: Date | null;

    /**
     * 创建者Id
     *
     * @type {number}
     * @memberof SysUser
     */
    createUserId?: number | null;

    /**
     * 创建者姓名
     *
     * @type {string}
     * @memberof SysUser
     */
    createUserName?: string | null;

    /**
     * 修改者Id
     *
     * @type {number}
     * @memberof SysUser
     */
    updateUserId?: number | null;

    /**
     * 修改者姓名
     *
     * @type {string}
     * @memberof SysUser
     */
    updateUserName?: string | null;

    /**
     * 软删除
     *
     * @type {boolean}
     * @memberof SysUser
     */
    isDelete?: boolean;

    /**
     * 租户Id
     *
     * @type {number}
     * @memberof SysUser
     */
    tenantId?: number | null;

    /**
     * 账号
     *
     * @type {string}
     * @memberof SysUser
     */
    account: string;

    /**
     * 真实姓名
     *
     * @type {string}
     * @memberof SysUser
     */
    realName?: string | null;

    /**
     * 昵称
     *
     * @type {string}
     * @memberof SysUser
     */
    nickName?: string | null;

    /**
     * 头像
     *
     * @type {string}
     * @memberof SysUser
     */
    avatar?: string | null;

    /**
     * @type {GenderEnum}
     * @memberof SysUser
     */
    sex?: GenderEnum;

    /**
     * 年龄
     *
     * @type {number}
     * @memberof SysUser
     */
    age?: number;

    /**
     * 出生日期
     *
     * @type {Date}
     * @memberof SysUser
     */
    birthday?: Date | null;

    /**
     * 民族
     *
     * @type {string}
     * @memberof SysUser
     */
    nation?: string | null;

    /**
     * 手机号码
     *
     * @type {string}
     * @memberof SysUser
     */
    phone?: string | null;

    /**
     * @type {CardTypeEnum}
     * @memberof SysUser
     */
    cardType?: CardTypeEnum;

    /**
     * 身份证号
     *
     * @type {string}
     * @memberof SysUser
     */
    idCardNum?: string | null;

    /**
     * 邮箱
     *
     * @type {string}
     * @memberof SysUser
     */
    email?: string | null;

    /**
     * 地址
     *
     * @type {string}
     * @memberof SysUser
     */
    address?: string | null;

    /**
     * @type {CultureLevelEnum}
     * @memberof SysUser
     */
    cultureLevel?: CultureLevelEnum;

    /**
     * 政治面貌
     *
     * @type {string}
     * @memberof SysUser
     */
    politicalOutlook?: string | null;

    /**
     * 毕业院校
     *
     * @type {string}
     * @memberof SysUser
     */
    college?: string | null;

    /**
     * 办公电话
     *
     * @type {string}
     * @memberof SysUser
     */
    officePhone?: string | null;

    /**
     * 紧急联系人
     *
     * @type {string}
     * @memberof SysUser
     */
    emergencyContact?: string | null;

    /**
     * 紧急联系人电话
     *
     * @type {string}
     * @memberof SysUser
     */
    emergencyPhone?: string | null;

    /**
     * 紧急联系人地址
     *
     * @type {string}
     * @memberof SysUser
     */
    emergencyAddress?: string | null;

    /**
     * 个人简介
     *
     * @type {string}
     * @memberof SysUser
     */
    introduction?: string | null;

    /**
     * 排序
     *
     * @type {number}
     * @memberof SysUser
     */
    orderNo?: number;

    /**
     * @type {StatusEnum}
     * @memberof SysUser
     */
    status?: StatusEnum;

    /**
     * 备注
     *
     * @type {string}
     * @memberof SysUser
     */
    remark?: string | null;

    /**
     * @type {AccountTypeEnum}
     * @memberof SysUser
     */
    accountType?: AccountTypeEnum;

    /**
     * 直属机构Id
     *
     * @type {number}
     * @memberof SysUser
     */
    orgId?: number;

    /**
     * 直属主管Id
     *
     * @type {number}
     * @memberof SysUser
     */
    managerUserId?: number | null;

    /**
     * 职位Id
     *
     * @type {number}
     * @memberof SysUser
     */
    posId?: number;

    /**
     * 工号
     *
     * @type {string}
     * @memberof SysUser
     */
    jobNum?: string | null;

    /**
     * 职级
     *
     * @type {string}
     * @memberof SysUser
     */
    posLevel?: string | null;

    /**
     * 职称
     *
     * @type {string}
     * @memberof SysUser
     */
    posTitle?: string | null;

    /**
     * 擅长领域
     *
     * @type {string}
     * @memberof SysUser
     */
    expertise?: string | null;

    /**
     * 办公区域
     *
     * @type {string}
     * @memberof SysUser
     */
    officeZone?: string | null;

    /**
     * 办公室
     *
     * @type {string}
     * @memberof SysUser
     */
    office?: string | null;

    /**
     * 入职日期
     *
     * @type {Date}
     * @memberof SysUser
     */
    joinDate?: Date | null;

    /**
     * 最新登录Ip
     *
     * @type {string}
     * @memberof SysUser
     */
    lastLoginIp?: string | null;

    /**
     * 最新登录地点
     *
     * @type {string}
     * @memberof SysUser
     */
    lastLoginAddress?: string | null;

    /**
     * 最新登录时间
     *
     * @type {Date}
     * @memberof SysUser
     */
    lastLoginTime?: Date | null;

    /**
     * 最新登录设备
     *
     * @type {string}
     * @memberof SysUser
     */
    lastLoginDevice?: string | null;

    /**
     * 电子签名
     *
     * @type {string}
     * @memberof SysUser
     */
    signature?: string | null;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Packets
{
    public enum RecvOpcodes : ushort
    {
        World_Select = 0x0043,
        Check_User_Limit = 0x001D,
        World_Info_Request = 0x0022,
        World_Info_Rerequest = 0x0023,
        Migrate = 0x0027,
        Check_Name_Duplicate = 0x0028,
        Delete_Character = 0x002C,
        Login = 0x0001,
        Select_Character = 0x0047,
        Part_Time_Job = 0x003A,
        Create_Character = 0x0045,

        Change_Map = 0x0051,
        Change_Channel = 0x0052,

        Enter_CashShop = 0x0054,
        Enter_Farm = 0x004B,
        Enter_Azwan = 0x0049,
        Enter_Azwan_Event = 0x004A,

        Leave_Azwan = 0x004B,
        Enter_Battle = 0x0999,
        Enter_Battle_Party = 0x0999,
        Leave_Battle = 0x0999,

        Player_Movement = 0x005E,
        Chair_Cancel = 0x0060,
        Chair_Use = 0x0061,

        Attack_Melee = 0x0062,
        Attack_Ranged = 0x0063,
        Attack_Magic = 0x0064,

        Player_Damage = 0x0068,
        Player_Chat = 0x006A,
        Player_Emote = 0x006C,

        NPC_Talk = 0x007E,
        NPC_Talk_More = 0x0080,
        NPC_Shop = 0x0081,
        NPC_Trunk = 0x0083,

        Life_Movement = 0x01EA,

        Inventory_Sort = 0x0090,
        Inventory_Gather = 0x0091,
        Inventory_Change_Slot = 0x0092,
        Inventory_Use_Item = 0x0096,

        Heartbeat = 0x0030,
        Start = 0x0038,
        Validate = 0x003F
    }

    public enum SendOpcodes : ushort
    {
        Login_Status = 0x0001,
        World_Status = 0x0004,
        World_Select_Result = 0x000A,
        Migrate = 0x000B,
        Check_Name_Result = 0x000C,
        Create_Character_Result = 0x000D,
        Delete_Character_Result = 0x000E,
        Remigrate = 0x0010,
        PIC_Error = 0x0024,
        Heartbeat_Response = 0x0015,
        World_Information = 0x0009,
        Login_Background = 0x0111,

        Stats_Update = 0x0027,

        Channel_Change = 0x00010,

        Field_Enter = 0x010C,
        Player_Spawn = 0x0143,
        Player_Despawn = 0x0144,
        Player_Chat = 0x0145,

        Inventory_Operation = 0x0025,
        Inventory_Grow = 0x0026,

        Player_Update = 0x0027,
        Player_Buff = 0x0028,
        Player_Debuff = 0x0029,
        Player_Temp_Stats = 0x002A,
        Player_Temp_Stats_Reset = 0x002B,

        Update_Skills = 0x002C,
        Update_Stolen_Skills = 0x002D,

        Fame_Response = 0x0032,
        Show_Status = 0x0033,
        Show_Notes = 0x0035,
        Show_Quest_Complete = 0x003E,

        Cash_Shop_Enter = 0x010F,
        Cash_Shop_Update = 0x02F5,
        Cash_Shop_Opeartion = 0x02F6,
        Cash_Shop = 0x0309,
        Cash_Shop_Use = 0x0012,

        Player_Movement = 0x0192,
        Chair_Use = 0x01A4,
        Chair_Cancel = 0x014F,

        Player_Damage = 0x0113,

        Monster_Spawn = 0x023C,
        Monster_Kill = 0x023D,
        Monster_Control_Spawn = 0x023E,
        Monster_Movement = 0x0240,
        Monster_Movement_Response = 0x0241,
        Monster_Status_Apply = 0x0243,
        Monster_Status_Cancel = 0x0244,
        Monster_Damage = 0x0247,
        Monster_CRC_Change = 0x024A,
        Monster_Show_HP = 0x024B,
        Monster_Catch = 0x024E,
        Monster_Properties = 0x01B8,
        Monster_Talk_Remove = 0x01B9,
        Monster_Talk = 0x01BA,


        NPC_Spawn = 0x0265
    }
}

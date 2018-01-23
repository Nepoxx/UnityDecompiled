// Decompiled with JetBrains decompiler
// Type: UnityEditor.MenuItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>The MenuItem attribute allows you to add menu items to the main menu and inspector context menus.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  [RequiredByNativeCode]
  public sealed class MenuItem : Attribute
  {
    public string menuItem;
    public bool validate;
    public int priority;

    /// <summary>
    ///   <para>Creates a menu item and invokes the static function following it, when the menu item is selected.</para>
    /// </summary>
    /// <param name="itemName">The itemName is the menu item represented like a pathname.
    /// For example the menu item could be "GameObject/Do Something".</param>
    /// <param name="isValidateFunction">If isValidateFunction is true, this is a validation
    /// function and will be called before invoking the menu function with the same itemName.</param>
    /// <param name="priority">The order by which the menu items are displayed.</param>
    public MenuItem(string itemName)
      : this(itemName, false)
    {
    }

    /// <summary>
    ///   <para>Creates a menu item and invokes the static function following it, when the menu item is selected.</para>
    /// </summary>
    /// <param name="itemName">The itemName is the menu item represented like a pathname.
    /// For example the menu item could be "GameObject/Do Something".</param>
    /// <param name="isValidateFunction">If isValidateFunction is true, this is a validation
    /// function and will be called before invoking the menu function with the same itemName.</param>
    /// <param name="priority">The order by which the menu items are displayed.</param>
    public MenuItem(string itemName, bool isValidateFunction)
      : this(itemName, isValidateFunction, !itemName.StartsWith("GameObject/Create Other") ? 1000 : 10)
    {
    }

    /// <summary>
    ///   <para>Creates a menu item and invokes the static function following it, when the menu item is selected.</para>
    /// </summary>
    /// <param name="itemName">The itemName is the menu item represented like a pathname.
    /// For example the menu item could be "GameObject/Do Something".</param>
    /// <param name="isValidateFunction">If isValidateFunction is true, this is a validation
    /// function and will be called before invoking the menu function with the same itemName.</param>
    /// <param name="priority">The order by which the menu items are displayed.</param>
    public MenuItem(string itemName, bool isValidateFunction, int priority)
      : this(itemName, isValidateFunction, priority, false)
    {
    }

    internal MenuItem(string itemName, bool isValidateFunction, int priority, bool internalMenu)
    {
      this.menuItem = !internalMenu ? itemName : "internal:" + itemName;
      this.validate = isValidateFunction;
      this.priority = priority;
    }
  }
}

   M S B A M L   `   `   `  ����  ZadanieEversis    MVVMExample.MainWindow   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Main Window��5   	   $	��350��6   $	��500��6*   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��auto �6   .+�5   
   &   System.Windows.Controls.DataGrid     
IsReadOnly$
 True��6U      AutoGenerateColumns$
 True��6g   ��6   �� *Data+ �� SelectedItem 60   �� *SelectedRow+6g   �� 5   
   $
��Load��6   $��1��6@   $
��Left=�6M   $��146,10,0,10q�6h   $	��131��6}   ��6    �� *LoadCommand+6}   �� 5   
   $
��Edit��6   $��1��6@   $
��Left=�6M   $��
10,10,0,10q�6h   $	��131��6|   ��6    �� *EditCommand+6|   �� 5   
   $��Close��6   $��1��6B   $
��Left=�6O   $��360,10,0,10q�6j   $	��131��6   ��6!   �� *CloseCommand+6   5      5      
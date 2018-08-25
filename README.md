# プリザンター Linux 版

プリザンター Linux 版 アルファ版のデバッグへのご協力まことにありがとうございます。  
本プロダクトは開発途中のものであり、まだ多くの不具合が含まれております。不具合を発見された場合は issue にてお知らせください。

# 製品版の README
Windwos 版の README のコピーは[こちら](README_PLEASANTER.md)です。  
製品版のリポジトリは[こちら](https://github.com/Implem/Implem.Pleasanter)です。

## デバッグ環境構築方法

プリザンター Linux 版は、まだアルファ版でありインストーラーなどが整っていません。  
手動で動作環境を構築する必要があります。**※一部 Windows 製品版のインストーラーを使用する手順があります。**

# ソースコードの入手

# 開発環境の構築（Windows）

## 開発環境を構成する（Windows）

- Visual Studio 2017 Version 15.7.4
- .NET Core 2.1.301（手動インストールが必要 2018/06/26現在）
参考： [.NET Downloads for Windows](https://www.microsoft.com/net/download/windows)

## SQL Server を構成する（Windows）

参考： [プリザンターの使い方マニュアル](https://github.com/Implem/Implem.Pleasanter/wiki/%E3%83%97%E3%83%AA%E3%82%B6%E3%83%B3%E3%82%BF%E3%83%BC%E3%81%AE%E4%BD%BF%E3%81%84%E6%96%B9%E3%83%9E%E3%83%8B%E3%83%A5%E3%82%A2%E3%83%AB)  
次のどちらの方法でも構いません。

- [プリザンターのインストール](https://github.com/Implem/Implem.Pleasanter/wiki/%E3%83%97%E3%83%AA%E3%82%B6%E3%83%B3%E3%82%BF%E3%83%BC%E3%81%AE%E3%82%A4%E3%83%B3%E3%82%B9%E3%83%88%E3%83%BC%E3%83%AB)
- [プリザンターのデータベースを手動で構成する（高度な設定）](https://github.com/Implem/Implem.Pleasanter/wiki/%E3%83%97%E3%83%AA%E3%82%B6%E3%83%B3%E3%82%BF%E3%83%BC%E3%81%AE%E3%83%87%E3%83%BC%E3%82%BF%E3%83%99%E3%83%BC%E3%82%B9%E3%82%92%E6%89%8B%E5%8B%95%E3%81%A7%E6%A7%8B%E6%88%90%E3%81%99%E3%82%8B%EF%BC%88%E9%AB%98%E5%BA%A6%E3%81%AA%E8%A8%AD%E5%AE%9A%EF%BC%89)

# 実行環境の構築（Linux）

プリザンター(Linux アルファ版)は Windows 上の VirtualBox にインストールした Ubuntu で開発しています。

- VirtualBox バージョン 5.2.12
- Ubuntu 18.04

## 実行環境を構成する（Linux）

- .NET Core 2.1.301（手動インストールが必要）
参考： [Install .NET Core SDK on Linux Ubuntu 18.04](https://www.microsoft.com/net/download/linux-package-manager/ubuntu18-04/sdk-current)
- SQL Server 2017（手動インストールが必要）
参考： [クイック スタート: SQL Server をインストールし、Ubuntu でデータベースを作成](https://docs.microsoft.com/ja-jp/sql/linux/quickstart-install-connect-ubuntu?view=sql-server-linux-2017)
- GDI+ （手動インストールが必要）  
参考：  
```
ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
apt-get install -y libgdiplus
apt-get install -y libc6-dev
```

## SQL Server を構成する（Linux）

1. プリザンターのインストーラー（従来のもの）を使用し、Windows 上にプリザンターを構築します。
1. Linux 上の SQL Server を Windows マシン上から接続できるようにします。    
1. **PleasanterNetCore\Implem.Pleasanter\App_Data\Parameters\Rds.json** ファイルを書き換え、接続先を Linux 上の SQL Server にします。
1. **PleasanterNetCore\Implem.Pleasanter\App_Data\Definitions\definition_Sql.xlsm** ファイルを .NET Core 版（Linux版）のソースコードのもので上書きします。
1. プリザンターのインストーラーを使用して、Linux 上の SQL Server を構成します。

参考： [プリザンターの使い方マニュアル](https://github.com/Implem/Implem.Pleasanter/wiki/%E3%83%97%E3%83%AA%E3%82%B6%E3%83%B3%E3%82%BF%E3%83%BC%E3%81%AE%E4%BD%BF%E3%81%84%E6%96%B9%E3%83%9E%E3%83%8B%E3%83%A5%E3%82%A2%E3%83%AB)  
次のどちらの方法でも構いません。

- [プリザンターのインストール](https://github.com/Implem/Implem.Pleasanter/wiki/%E3%83%97%E3%83%AA%E3%82%B6%E3%83%B3%E3%82%BF%E3%83%BC%E3%81%AE%E3%82%A4%E3%83%B3%E3%82%B9%E3%83%88%E3%83%BC%E3%83%AB)
- [プリザンターのデータベースを手動で構成する（高度な設定）](https://github.com/Implem/Implem.Pleasanter/wiki/%E3%83%97%E3%83%AA%E3%82%B6%E3%83%B3%E3%82%BF%E3%83%BC%E3%81%AE%E3%83%87%E3%83%BC%E3%82%BF%E3%83%99%E3%83%BC%E3%82%B9%E3%82%92%E6%89%8B%E5%8B%95%E3%81%A7%E6%A7%8B%E6%88%90%E3%81%99%E3%82%8B%EF%BC%88%E9%AB%98%E5%BA%A6%E3%81%AA%E8%A8%AD%E5%AE%9A%EF%BC%89)

## Linux 向けの publish（発行）（Windows）

Visual Studio から Linux 実行環境へ配置するバイナリの発行を行います。

1. **Implem.Pleasanter** プロジェクト > 発行
1. 新しいプロファイル
1. フォルダー
1. 詳細設定
1. [ 構成 ] -> リリース
1. [ ターゲットフレームワーク ] → .netcoreapp2.1
1. [ 配置モード ] → 自己完結
1. [ ターゲットランタイム ] → linux-x64
1. [保存]
1. [プロファイルの作成]
1. [発行]

## Linux への配置

発行したバイナリを実行環境 Linux 上にコピーします。

# 実行

**実行コマンド**
```
dotnet Implem.Pleasanter.dll
```

**ブラウザでアクセス**
```
http://localhost:5000/
```
# デバッグする

# issue を立てる

プリザンター Linux 版 アルファ版のデバッグへのご協力まことにありがとうございます。  
本プロダクトは開発途中のものであり、まだ多くの不具合が含まれております。不具合を発見された場合は issue にてお知らせください。

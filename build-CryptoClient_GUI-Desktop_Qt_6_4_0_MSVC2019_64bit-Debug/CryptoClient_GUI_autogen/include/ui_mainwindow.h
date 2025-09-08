/********************************************************************************
** Form generated from reading UI file 'mainwindow.ui'
**
** Created by: Qt User Interface Compiler version 6.4.0
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_MAINWINDOW_H
#define UI_MAINWINDOW_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtWidgets/QApplication>
#include <QtWidgets/QComboBox>
#include <QtWidgets/QGroupBox>
#include <QtWidgets/QLabel>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QMenu>
#include <QtWidgets/QMenuBar>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QStatusBar>
#include <QtWidgets/QTabWidget>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_MainWindow
{
public:
    QWidget *centralwidget;
    QTabWidget *tabWidget;
    QWidget *tab;
    QLabel *labeltest1;
    QLabel *labeltest2;
    QPushButton *pushButton;
    QWidget *tabSymbol;
    QLabel *label;
    QLabel *lblMakerName;
    QLabel *label_2;
    QLabel *lblTakerName;
    QComboBox *cmbSymbols;
    QGroupBox *groupBox;
    QMenuBar *menubar;
    QMenu *menuMain;
    QStatusBar *statusbar;

    void setupUi(QMainWindow *MainWindow)
    {
        if (MainWindow->objectName().isEmpty())
            MainWindow->setObjectName("MainWindow");
        MainWindow->resize(800, 600);
        centralwidget = new QWidget(MainWindow);
        centralwidget->setObjectName("centralwidget");
        tabWidget = new QTabWidget(centralwidget);
        tabWidget->setObjectName("tabWidget");
        tabWidget->setGeometry(QRect(0, 0, 671, 541));
        tab = new QWidget();
        tab->setObjectName("tab");
        labeltest1 = new QLabel(tab);
        labeltest1->setObjectName("labeltest1");
        labeltest1->setGeometry(QRect(30, 30, 261, 241));
        labeltest2 = new QLabel(tab);
        labeltest2->setObjectName("labeltest2");
        labeltest2->setGeometry(QRect(400, 30, 261, 241));
        pushButton = new QPushButton(tab);
        pushButton->setObjectName("pushButton");
        pushButton->setGeometry(QRect(300, 420, 80, 24));
        tabWidget->addTab(tab, QString());
        tabSymbol = new QWidget();
        tabSymbol->setObjectName("tabSymbol");
        label = new QLabel(tabSymbol);
        label->setObjectName("label");
        label->setGeometry(QRect(10, 60, 71, 21));
        lblMakerName = new QLabel(tabSymbol);
        lblMakerName->setObjectName("lblMakerName");
        lblMakerName->setGeometry(QRect(60, 60, 71, 21));
        label_2 = new QLabel(tabSymbol);
        label_2->setObjectName("label_2");
        label_2->setGeometry(QRect(340, 60, 71, 21));
        lblTakerName = new QLabel(tabSymbol);
        lblTakerName->setObjectName("lblTakerName");
        lblTakerName->setGeometry(QRect(380, 60, 71, 21));
        cmbSymbols = new QComboBox(tabSymbol);
        cmbSymbols->setObjectName("cmbSymbols");
        cmbSymbols->setGeometry(QRect(10, 20, 141, 21));
        groupBox = new QGroupBox(tabSymbol);
        groupBox->setObjectName("groupBox");
        groupBox->setGeometry(QRect(30, 90, 261, 271));
        tabWidget->addTab(tabSymbol, QString());
        MainWindow->setCentralWidget(centralwidget);
        menubar = new QMenuBar(MainWindow);
        menubar->setObjectName("menubar");
        menubar->setGeometry(QRect(0, 0, 800, 21));
        menuMain = new QMenu(menubar);
        menuMain->setObjectName("menuMain");
        MainWindow->setMenuBar(menubar);
        statusbar = new QStatusBar(MainWindow);
        statusbar->setObjectName("statusbar");
        MainWindow->setStatusBar(statusbar);

        menubar->addAction(menuMain->menuAction());

        retranslateUi(MainWindow);
        QObject::connect(pushButton, &QPushButton::clicked, MainWindow, qOverload<>(&QMainWindow::show));

        tabWidget->setCurrentIndex(0);


        QMetaObject::connectSlotsByName(MainWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MainWindow)
    {
        MainWindow->setWindowTitle(QCoreApplication::translate("MainWindow", "MainWindow", nullptr));
        labeltest1->setText(QCoreApplication::translate("MainWindow", "TextLabel", nullptr));
        labeltest2->setText(QCoreApplication::translate("MainWindow", "TextLabel", nullptr));
        pushButton->setText(QCoreApplication::translate("MainWindow", "PushButton", nullptr));
        tabWidget->setTabText(tabWidget->indexOf(tab), QCoreApplication::translate("MainWindow", "Tab 1", nullptr));
        label->setText(QCoreApplication::translate("MainWindow", "Maker:", nullptr));
        lblMakerName->setText(QCoreApplication::translate("MainWindow", "Market M", nullptr));
        label_2->setText(QCoreApplication::translate("MainWindow", "Taker:", nullptr));
        lblTakerName->setText(QCoreApplication::translate("MainWindow", "Market T", nullptr));
        groupBox->setTitle(QString());
        tabWidget->setTabText(tabWidget->indexOf(tabSymbol), QCoreApplication::translate("MainWindow", "Symbol", nullptr));
        menuMain->setTitle(QCoreApplication::translate("MainWindow", "Main", nullptr));
    } // retranslateUi

};

namespace Ui {
    class MainWindow: public Ui_MainWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MAINWINDOW_H

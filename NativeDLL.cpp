#include "stdafx.h"

// __declspec(dllexport)

// mangling
extern "C" {

    int Insert (int p1, int p2) {

        return (p1 + p2);
    }

    int Delete (char* src1, char* src2, char* dst) {

        return true;
    }

    int Member (char* src1, char* src2, char* dst) {

        return true;
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables;

namespace ConsoleApp1
{
    public class SimplexMethod
    { 
        public double[,] _tableWithAddVars;
        private double[,] _startTable;
        private int _startRows;
        private int _startCols;
        private List<int> _basisVars;
        private int _allVarsCount;

        public SimplexMethod(double[,] startTable  )
      {
          _basisVars = new List<int>();
          _startTable = startTable;
          _startRows = _startTable.GetLength(0);
          _startCols = startTable.GetLength(1);
          _tableWithAddVars = new double[_startRows, _startCols + _startRows - 1];
          _allVarsCount = _startCols + _startRows - 2;
          FillExtendedArrayByBasisVars();
      }

      private void FillExtendedArrayByBasisVars()
      {
          for (int row = 0; row < _startRows; row++)
          {
              for (int col = 0; col < _tableWithAddVars.GetLength(1); col++)
              {
                  if (col >= _startCols)
                  {
                      _tableWithAddVars[row, col] = 0;
                  }
                  else _tableWithAddVars[row, col] = _startTable[row, col];
              }

              if (_startCols + row < _tableWithAddVars.GetLength(1))
              {
                  _tableWithAddVars[row, row + _startCols] = 1;
                  _basisVars.Add(row+_startCols);
              }
          }
      }


      public double[] CalculateSimplexTable()
      {
          double[] answers = new double[_tableWithAddVars.GetLength(0) - 1];
          PrintSimplexTable();
          Console.WriteLine("COL : " + FindNewTableCol());
          Console.WriteLine("ROW : " + FindNewTableRow(FindNewTableCol()));

          while (CheckIfFindedMaxValues())
          {
              int newCol = FindNewTableCol();
              int newRow = FindNewTableRow(newCol);
              double[,] newTable = new double[_tableWithAddVars.GetLength(0), _tableWithAddVars.GetLength(1)];
              double crossedValue = _tableWithAddVars[newRow, newCol];
              _basisVars[newRow] = newCol;

              for (int coll = 0; coll < newTable.GetLength(1); coll++)
              {
                  newTable[newRow, coll] = _tableWithAddVars[newRow, coll] / crossedValue;
              }

              for (int row = 0; row < _startRows; row++)
              {
                  if (row == newRow)
                  {
                      continue;
                  }

                  for (int col = 0; col < newTable.GetLength(1); col++)
                  {
                      newTable[row, col] = _tableWithAddVars[row, col] -
                                           _tableWithAddVars[row, newCol] * newTable[newRow, col];
                  }
              }

              _tableWithAddVars = newTable;
              PrintSimplexTable();
              Console.WriteLine("COL : " + FindNewTableCol());
              Console.WriteLine("ROW : " + FindNewTableRow(FindNewTableCol()));
          }

          for (int itter = 0; itter < answers.Length; itter++)
          {
              answers[itter] = _tableWithAddVars[itter, 0];
          }

          return answers;
      }

     

      public void PrintSimplexTable()
      {
         
          int additionalVarsCount = _tableWithAddVars.GetLength(1) - _startTable.GetLength(1);
          var table = new ConsoleTable("Res", "x1", "x2", "s1", "s2","s3");

          for (int row = 0; row < _tableWithAddVars.GetLength(0); row++)
          {
              string[] rowVals = FromDoubleToStringArr(GetRow(_tableWithAddVars, row));
              table.AddRow(rowVals);
          }

          table.Write();
      }

      public void PrintAnswers(double [] answers)
      {
          var table = new ConsoleTable("x2", "x1","x1");
          table.AddRow(FromDoubleToStringArr(answers));
          table.Write();

      }
      
      public double[] GetRow(double[,] matrix, int rowNumber)
      {
          return Enumerable.Range(0, matrix.GetLength(1))
              .Select(x => matrix[rowNumber, x])
              .ToArray();
      }

      private string[] FromDoubleToStringArr(double[] arr)
      {
          string[] ar = new string[arr.GetLength(0)];
          for (int itter = 0; itter < ar.GetLength(0); itter++)
          {
              ar[itter] = Convert.ToString(arr[itter]);
          }

          return ar;
      }

      public int FindNewTableRow(int col)
      {
          int newColBasis = col;
          int minValueIndex = 0;
          double minValue = 1000000000;
          for (int row = 0; row < _tableWithAddVars.GetLength(0)-1; row++)
          {
              if (_tableWithAddVars[row, newColBasis] > 0 &&
                  _tableWithAddVars[row, 0] / _tableWithAddVars[row, newColBasis] < minValue)
              {
                  minValueIndex = row;
                  minValue = _tableWithAddVars[row, 0] / _tableWithAddVars[row, newColBasis];
              }
          }

          return minValueIndex;
      }

      public int FindNewTableCol()
      {
          List<int> nonBasisVarsIndexes = GetNonBasisVarsIndexes();
          int mainCol = 1; 
          for (int cols = 1; cols < _tableWithAddVars.GetLength(1); cols++)
          {
              if (nonBasisVarsIndexes.Contains(cols))
              {
                  double value = _tableWithAddVars[_startRows - 1, cols];
                  if (value < 0)
                  {
                      if (Math.Abs(_tableWithAddVars[_startRows - 1, cols]) >
                          Math.Abs(_tableWithAddVars[_startRows - 1, mainCol]))
                      {
                          mainCol = cols;
                      }
                  }
              }
          }
          return mainCol;
      }

      private bool CheckIfFindedMaxValues()
      {
          for (int i = 1; i < _tableWithAddVars.GetLength(1); i++)
          {
              double va = _tableWithAddVars[_startRows - 1, i];
              if (va < 0)
              {
                  return true;
              }
          }
          return false;
      }

      private List<int> GetNonBasisVarsIndexes()
      {
          List<int> nonBasisIndexed = new List<int>();
          for (int itter = 1; itter <= _allVarsCount; itter++)
          {
              if (!_basisVars.Contains(itter))
              {
                  nonBasisIndexed.Add(itter);
              }
          }

          return nonBasisIndexed;
      }
    }
}
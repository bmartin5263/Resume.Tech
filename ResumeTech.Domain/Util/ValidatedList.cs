// namespace ResumeTech.Domain.Util; 
//
// public interface ValidatedList<T> : IList<T> {
//     
//     void Insert(int index, T item) {
//         CheckCanInsert(index, item);
//         Insert(index, item);
//     }
//
//     void IList<T>.RemoveAt(int index) {
//         CheckCanRemoveAt(index);
//         RemoveAt(index);
//     }
//
//     void ICollection<T>.Add(T item) {
//         CheckCanAdd(item);
//         Add(item);
//     }
//
//     bool ICollection<T>.Remove(T item) {
//         CheckCanRemove(item);
//         return Remove(item);
//     }
//
//     void CheckCanInsert(int index, T item);
//     void CheckCanAdd(T item);
//     void CheckCanRemoveAt(int index);
//     void CheckCanRemove(T item);
//
//     void IList<T>.Insert(int index, T item);
//     new void Add(T item);
//     new void RemoveAt(int index);
//     new bool Remove(T item);
// }
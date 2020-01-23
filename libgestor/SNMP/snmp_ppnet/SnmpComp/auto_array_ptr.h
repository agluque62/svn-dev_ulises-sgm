// Source: "Using Exceptions Effectively: Part I: Coping With Exceptions"
// by Jack W. Reeves, ©C++ Report.
// Available: http://www.cs.wustl.edu/~schmidt/adaptive/cs562/exceptions/e1.rtf

#ifndef	__auto_array_ptr
#define __auto_array_ptr

template<class X>
class auto_array_ptr {
	X* p_;
public:
	auto_array_ptr(X* p = nil) throw() : p_(p) {}
	auto_array_ptr(auto_array_ptr<X>& ap) throw() :
		p_(ap.release()) {}
	~auto_array_ptr() {delete[] p_;}
	 void operator=(auto_array_ptr<X>& rhs);

	X& operator*() throw() {return *p_;}
	X& operator[](int i) throw() {return p_[i];}
	X operator[](int i) const throw() {return p_[i];}
	X* get() const throw() {return p_;}
	X* release() throw() {return reset(nil);}
	X* reset(X* p) throw() {X* tp = p_; p_ = p; return tp;}

	static void remove(X*& x) {X* tp=x; x=nil; delete[] tp;}
};

#endif
